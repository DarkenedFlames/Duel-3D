using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

// Apply this drawer to any field that is an IInstruction type (or derived types)
[CustomPropertyDrawer(typeof(IInstruction), true)]
public class InstructionBindingDrawer : PropertyDrawer
{
    // ... [Static constructor to find types remains the same as previous example] ...
    private static readonly Type[] InstructionTypes;

    static InstructionBindingDrawer()
    {
        InstructionTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && !t.IsInterface && typeof(IInstruction).IsAssignableFrom(t) && t.IsDefined(typeof(SerializableAttribute)))
            .ToArray();
    }
    // ...

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // This is where you draw the dropdown button for the IInstruction field
        EditorGUI.BeginProperty(position, label, property);

        Rect buttonRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        string currentTypeName = property.managedReferenceValue?.GetType().Name ?? "(Null)";
        
        if (EditorGUI.DropdownButton(buttonRect, new GUIContent($"Select Type: {currentTypeName}"), FocusType.Keyboard))
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Null"), property.managedReferenceValue == null, () =>
            {
                property.managedReferenceValue = null;
                property.serializedObject.ApplyModifiedProperties();
            });

            foreach (Type type in InstructionTypes)
            {
                menu.AddItem(new GUIContent(type.Name), property.managedReferenceValue?.GetType() == type, () =>
                {
                    property.managedReferenceValue = Activator.CreateInstance(type);
                    property.serializedObject.ApplyModifiedProperties();
                });
            }
            menu.ShowAsContext();
        }

        // Space for the dropdown button.
        Rect subPropertyRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height - EditorGUIUtility.singleLineHeight);

        // Draw the sub-properties of the selected instance if it exists.
        if (property.managedReferenceValue != null)
        {
            // We pass an empty label because the "Type" label is handled by the dropdown.
            EditorGUI.PropertyField(subPropertyRect, property, GUIContent.none, true);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float baseHeight = EditorGUIUtility.singleLineHeight; // Height of the dropdown button
        if (property.managedReferenceValue != null)
        {
            // Add the height of the nested properties if an instance exists
            // The true parameter makes it calculate height for nested fields
            return baseHeight + EditorGUI.GetPropertyHeight(property, true);
        }
        return baseHeight;
    }
}
