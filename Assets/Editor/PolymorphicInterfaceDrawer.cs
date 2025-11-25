using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

public abstract class PolymorphicInterfaceDrawer : PropertyDrawer
{
    protected abstract Type InterfaceType { get; }

    private Type[] _implTypes;
    private string[] _implNames;

    private void Init()
    {
        if (_implTypes != null) return;

        _implTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && InterfaceType.IsAssignableFrom(t))
            .ToArray();

        _implNames = _implTypes.Select(t => t.Name).ToArray();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        Init();

        if (property.managedReferenceValue == null)
            return EditorGUIUtility.singleLineHeight;

        float height = EditorGUIUtility.singleLineHeight + 4;

        var iterator = property.Copy();
        var end = iterator.GetEndProperty();

        iterator.NextVisible(true);

        while (!SerializedProperty.EqualContents(iterator, end))
        {
            height += EditorGUI.GetPropertyHeight(iterator, true) + 2;
            iterator.NextVisible(false);
        }

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Init();

        EditorGUI.BeginProperty(position, label, property);

        var typeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        var valueRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2,
            position.width, position.height - EditorGUIUtility.singleLineHeight - 2);

        // Dropdown
        Type currentType = property.managedReferenceValue?.GetType();
        int currentIndex = Array.IndexOf(_implTypes, currentType);
        int newIndex = EditorGUI.Popup(typeRect, label.text, currentIndex, _implNames);

        if (newIndex != currentIndex)
            property.managedReferenceValue = Activator.CreateInstance(_implTypes[newIndex]);

        if (property.managedReferenceValue != null)
        {
            var iterator = property.Copy();
            var end = iterator.GetEndProperty();

            iterator.NextVisible(true);

            float y = valueRect.y;

            while (!SerializedProperty.EqualContents(iterator, end))
            {
                float h = EditorGUI.GetPropertyHeight(iterator, true);
                EditorGUI.PropertyField(new Rect(valueRect.x, y, valueRect.width, h), iterator, true);
                y += h + 2;

                iterator.NextVisible(false);
            }
        }

        EditorGUI.EndProperty();
    }
}
