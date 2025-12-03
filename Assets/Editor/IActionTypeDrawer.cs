using UnityEditor;
using UnityEngine;

public interface IActionTypeDrawer
{
    void Draw(SerializedProperty property, Rect position, GUIContent label);
}
