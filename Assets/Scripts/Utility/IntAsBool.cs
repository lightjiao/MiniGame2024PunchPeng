using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public struct IntAsBool
{
    [SerializeField]
    private int IntValue;

    public IntAsBool(int value)
    {
        IntValue = value;
    }

    public static implicit operator bool(IntAsBool self) => self.IntValue > 0;
    public static IntAsBool operator ++(IntAsBool self)
    {
        self.IntValue++;
        return self;
    }

    public static IntAsBool operator --(IntAsBool self)
    {
        self.IntValue--;
        return self;
    }

    public override readonly string ToString()
    {
        return IntValue.ToString();
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(IntAsBool))]
public class IntAsBoolDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 获取结构体的 "value" 字段
        SerializedProperty valueProperty = property.FindPropertyRelative("IntValue");

        // 绘制一个整数字段
        EditorGUI.PropertyField(position, valueProperty, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // 返回字段的高度
        return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("IntValue"), label);
    }
}
#endif