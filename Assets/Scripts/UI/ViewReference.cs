using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace SimpleMetalMax
{
    public partial class ViewReference : MonoBehaviour
    {
        [SerializeField]
        [TableList] //
        [ChildGameObjectsOnly(IncludeSelf = true)]
        public ReferenceItem[] references;


        [Button]
        private void GenerateCode()
        {
            var trimmedName = name;
            if (trimmedName.EndsWith("Form"))
            {
                trimmedName = trimmedName[..(trimmedName.Length - 4)];
            }

            var className = $"{trimmedName}View";
            var classCode = GenerateCodeByTemplate(className);
            var classFileName = $"{Application.dataPath}/../{CodePath}/{className}.cs";

            if (!File.Exists(classFileName))
            {
                File.Create(classFileName).Dispose();
            }

            File.WriteAllText(classFileName, string.Empty);
            File.WriteAllText(classFileName, classCode);
            Debug.Log($"Generate {className}.cs success.");
        }

        private string CodePath => Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(this)));
    }

    /// <summary>
    /// internal class part
    /// </summary>
    public partial class ViewReference
    {
        [Serializable]
        public class ReferenceItem
        {
            [ChildGameObjectsOnly] //
            [TableColumnWidth(180, resizable: false)]
            public GameObject item;

            [TableColumnWidth(200, resizable: false)]
            [ValueDropdown(nameof(ComponentsTypesString))]
            public string referenceTypeStr;

            public string fieldName;

            private IEnumerable ComponentsTypesString()
            {
                if (item == null) return null;

                var comps = item.GetComponents<Component>();
                var compsStr = new List<string>(comps.Length + 1) { "GameObject" };
                foreach (var comp in comps)
                {
                    compsStr.Add(comp.GetType().ToString().Split('.').Last());
                }

                return compsStr;
            }
        }
    }

    /// <summary>
    /// Generate code part
    /// </summary>
    public partial class ViewReference
    {
        private struct FieldInfo
        {
            public string FieldName;
            public int ReferenceIdx;
            public string FieldType;
        }

        private string GenerateCodeByTemplate(string className)
        {
            var fields = new List<FieldInfo>(references.Length);
            for (var i = 0; i < references.Length; i++)
            {
                fields.Add((new FieldInfo
                {
                    FieldName = string.IsNullOrEmpty(references[i].fieldName) ? references[i].item.name : references[i].fieldName,
                    ReferenceIdx = i,
                    FieldType = references[i].referenceTypeStr
                }));
            }

            var fieldNames = fields.Select(x => $"public {x.FieldType} {x.FieldName};\n");
            var fieldInit = fields.Select(x =>
            {
                if (x.FieldType == "GameObject")
                {
                    return $"{x.FieldName} = reference.references[{x.ReferenceIdx}].item;\n";
                }
                else
                {
                    return $"{x.FieldName} = reference.references[{x.ReferenceIdx}].item.GetComponent<{x.FieldType}>();\n";
                }
            });

            var template = GetCodeTemplate();
            template = template.Replace("__CLASS_NAME__", className);
            template = template.Replace("__FIELD_NAME__", string.Join("        ", fieldNames).TrimEnd('\n'));
            template = template.Replace("__FIELD_INIT__", string.Join("            ", fieldInit).TrimEnd('\n'));

            return template;
        }

        private string GetCodeTemplate()
        {
            return @"using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SimpleMetalMax
{
    public class __CLASS_NAME__ : AbstractView
    {
        __FIELD_NAME__

        protected override void Init(GameObject go)
        {
            var reference = go.GetComponent<ViewReference>();

            __FIELD_INIT__
        }
    }
}";
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Editor part
    /// </summary>
    public partial class ViewReference
    {
        private HashSet<GameObject> m_ReferenceHash;

        public bool ContainsGo(GameObject go)
        {
            if (m_ReferenceHash == null)
            {
                m_ReferenceHash = new();
                foreach (var item in references ?? Enumerable.Empty<ReferenceItem>())
                {
                    m_ReferenceHash.Add(item.item);
                }
            }

            return m_ReferenceHash.Contains(go);
        }
    }
#endif

#if UNITY_EDITOR
    [InitializeOnLoad]
    public static class ViewReferenceStyleHierarchy
    {
        static ViewReferenceStyleHierarchy()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
        }

        private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            if (EditorApplication.isPlaying || EditorApplication.isPaused)
            {
                return;
            }

            var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (go == null) return;

            ViewReference viewReference = null;
            if (EditorSceneManager.IsPreviewSceneObject(go))
            {
                viewReference = go.GetComponentInParent<ViewReference>();
            }
            else
            {
                var root = PrefabUtility.GetOutermostPrefabInstanceRoot(go);
                if (root != null)
                {
                    viewReference = root.GetComponent<ViewReference>();
                }
            }

            if (viewReference == null) return;
            if (viewReference.ContainsGo(go))
            {
                var color = go.activeInHierarchy ? new Color(1f, 0.0f, 1f, 0.7f) : new Color(0x9f / 255f, 0x70 / 255f, 0xb3 / 255f);
                var newStyle = new GUIStyle
                {
                    normal = new GUIStyleState() { textColor = color }
                };
                EditorGUI.LabelField(selectionRect, "      " + go.name, newStyle);
            }
        }
    }
#endif
}