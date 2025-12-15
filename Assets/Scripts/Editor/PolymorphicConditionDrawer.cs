using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Custom property drawer for IActionCondition that shows a dropdown to select condition type
/// and draws the selected condition's properties.
/// </summary>
[CustomPropertyDrawer(typeof(IActionCondition), true)]
public class PolymorphicConditionDrawer : PropertyDrawer
{
    static Type[] _types;
    static string[] _names;

    void Init()
    {
        if (_types != null) return;

        // Find all non-abstract IActionCondition implementations
        _types = AppDomain.CurrentDomain.GetAssemblies()
               .SelectMany(a => a.GetTypes())
               .Where(t => typeof(IActionCondition).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
               .ToArray();

        _names = _types.Select(t => FormatConditionName(t.Name)).ToArray();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        Init();

        var obj = property.managedReferenceValue;

        // If null — only the dropdown is drawn
        if (obj == null)
            return EditorGUIUtility.singleLineHeight;

        // Dropdown + all fields
        float height = EditorGUIUtility.singleLineHeight + 4;

        var copy = property.Copy();
        var end = copy.GetEndProperty();
        copy.NextVisible(true);

        while (!SerializedProperty.EqualContents(copy, end))
        {
            height += EditorGUI.GetPropertyHeight(copy, true) + 2;
            copy.NextVisible(false);
        }

        return height;
    }

    public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
    {
        Init();
        EditorGUI.BeginProperty(pos, label, property);

        float y = pos.y;
        var obj = property.managedReferenceValue;

        // Type selection dropdown
        Rect popupRect = new(pos.x, y, pos.width, EditorGUIUtility.singleLineHeight);

        int currentIndex = (obj == null)
            ? -1
            : Array.IndexOf(_types, obj.GetType());

        int newIndex = EditorGUI.Popup(popupRect, "Condition Type", currentIndex, _names);
        y += EditorGUIUtility.singleLineHeight + 4;

        // If changed — instantiate new type
        if (newIndex != currentIndex)
        {
            property.managedReferenceValue =
                newIndex >= 0 ? Activator.CreateInstance(_types[newIndex]) : null;

            EditorGUI.EndProperty();
            return;
        }

        if (obj == null)
        {
            EditorGUI.EndProperty();
            return;
        }

        // Draw all fields of the condition
        var copy = property.Copy();
        var end = copy.GetEndProperty();
        copy.NextVisible(true);

        while (!SerializedProperty.EqualContents(copy, end))
        {
            float h = EditorGUI.GetPropertyHeight(copy, true);
            EditorGUI.PropertyField(new Rect(pos.x, y, pos.width, h), copy, true);
            y += h + 2;
            copy.NextVisible(false);
        }

        EditorGUI.EndProperty();
    }

    static string FormatConditionName(string typeName)
    {
        // Remove "Condition" suffix if present
        if (typeName.EndsWith("Condition"))
            typeName = typeName[..^9]; // Remove last 9 characters ("Condition")

        // Add spaces before capital letters
        string result = "";
        for (int i = 0; i < typeName.Length; i++)
        {
            if (i > 0 && char.IsUpper(typeName[i]))
                result += " ";
            result += typeName[i];
        }

        return result;
    }
}
