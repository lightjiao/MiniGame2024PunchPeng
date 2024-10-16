using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class TransformEx
{
    public static Transform FindRecursive(this Transform self, string name)
    {
        if (self == null) return null;

        for (var i = 0; i < self.childCount; i++)
        {
            var child = self.transform.GetChild(i);
            if (child.name == name) return child;

            var childOfChild = child.FindRecursive(name);
            if (childOfChild != null)
            {
                return childOfChild;
            }
        }

        return null;
    }

    public static void DestroyAllChild(this Transform self)
    {
        if (self == null) return;

        var childCount = self.childCount;
        if (childCount <= 0) return;

        for (var i = childCount - 1; i >= 0; i--)
        {
            var child = self.transform.GetChild(i);
            Object.DestroyImmediate(child.gameObject);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Transform))]
public class TransformEditorEx : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        var transform = (Transform)target;
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.Vector3Field("World Position", transform.position);
        //EditorGUILayout.Vector3Field("World Rotation", transform.rotation.eulerAngles);
        EditorGUILayout.Vector3Field("Lossy Scale", transform.lossyScale);
        EditorGUI.EndDisabledGroup();

        DrawDefaultInspector();
    }
}
#endif