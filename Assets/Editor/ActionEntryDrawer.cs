using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ActionEntry), true)]
public class ActionEntryDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = 0;
        
        // Find the Hook field (exists in derived classes)
        SerializedProperty hookProp = property.FindPropertyRelative("Hook");
        if (hookProp != null)
        {
            height += EditorGUI.GetPropertyHeight(hookProp, true) + 2;
        }
        
        // Find the Action field (exists in base class)
        SerializedProperty actionProp = property.FindPropertyRelative("Action");
        if (actionProp != null)
        {
            height += EditorGUI.GetPropertyHeight(actionProp, true) + 2;
        }

        return height > 0 ? height : EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(pos, label, property);

        float y = pos.y;

        // Draw the Hook field first
        SerializedProperty hookProp = property.FindPropertyRelative("Hook");
        if (hookProp != null)
        {
            float h = EditorGUI.GetPropertyHeight(hookProp, true);
            EditorGUI.PropertyField(new Rect(pos.x, y, pos.width, h), hookProp, true);
            y += h + 2;
        }

        // Draw the Action field
        SerializedProperty actionProp = property.FindPropertyRelative("Action");
        if (actionProp != null)
        {
            float h = EditorGUI.GetPropertyHeight(actionProp, true);
            EditorGUI.PropertyField(new Rect(pos.x, y, pos.width, h), actionProp, true);
            y += h + 2;
        }

        EditorGUI.EndProperty();
    }
}
