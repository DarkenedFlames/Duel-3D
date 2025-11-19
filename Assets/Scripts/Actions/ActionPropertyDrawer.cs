using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[CustomPropertyDrawer(typeof(IGameAction), true)]
public class GameActionDrawer : PropertyDrawer
{
    static Type[] actionTypes;

    static string[] actionTypeNames;

    static GameActionDrawer()
    {
        // Automatically discover all IGameAction implementations
        actionTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && typeof(IGameAction).IsAssignableFrom(t))
            .ToArray();

        actionTypeNames = actionTypes.Select(t => t.Name).ToArray();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.managedReferenceValue == null)
            return EditorGUIUtility.singleLineHeight;

        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var typeRect = new Rect(position)
        {
            height = EditorGUIUtility.singleLineHeight
        };

        var valueRect = new Rect(position);
        valueRect.y += EditorGUIUtility.singleLineHeight + 2;
        valueRect.height = EditorGUI.GetPropertyHeight(property, label, true);

        // Determine currently selected type
        Type currentType = property.managedReferenceValue?.GetType();
        int currentIndex = Array.IndexOf(actionTypes, currentType);

        // Draw the type dropdown
        int newIndex = EditorGUI.Popup(typeRect, "Action Type", currentIndex, actionTypeNames);

        // If type changed → instantiate the new type
        if (newIndex != currentIndex && newIndex >= 0)
        {
            property.managedReferenceValue = Activator.CreateInstance(actionTypes[newIndex]);
        }

        // Draw the action’s fields
        if (property.managedReferenceValue != null)
            EditorGUI.PropertyField(valueRect, property, true);

        EditorGUI.EndProperty();
    }
}
