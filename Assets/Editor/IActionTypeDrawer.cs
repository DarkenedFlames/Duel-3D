using UnityEditor;
using UnityEngine;

public interface IActionTypeDrawer
{
    void Draw(SerializedProperty property, Rect position, GUIContent label);
    float GetHeight(SerializedProperty property, GUIContent label);
}
