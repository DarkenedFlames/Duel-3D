using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

public abstract class PolymorphicInterfaceDrawer : PropertyDrawer
{
    protected abstract Type InterfaceType { get; }

    private Type[] _implTypes;
    private string[] _implNames;
    private Dictionary<Type, IActionTypeDrawer> _customDrawers;

    private void Init()
    {
        if (_implTypes != null) return;

        _implTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && InterfaceType.IsAssignableFrom(t))
            .ToArray();

        _implNames = _implTypes.Select(t => t.Name).ToArray();

        // Auto-discover drawers
        _customDrawers = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && typeof(IActionTypeDrawer).IsAssignableFrom(t))
            .Select(t => new
            {
                DrawerType = t,
                Attr = t.GetCustomAttribute<ActionDrawerAttribute>()
            })
            .Where(x => x.Attr != null && x.DrawerType != null)
            .ToDictionary(
                x => x.Attr.TargetType,
                x => (IActionTypeDrawer)Activator.CreateInstance(x.DrawerType)
            );
    }

    protected void DrawDefault(SerializedProperty property, Rect position)
    {
        var iterator = property.Copy();
        var end = iterator.GetEndProperty();
        iterator.NextVisible(true);

        float y = position.y;
        while (!SerializedProperty.EqualContents(iterator, end))
        {
            float h = EditorGUI.GetPropertyHeight(iterator, true);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), iterator, true);
            y += h + 2;
            iterator.NextVisible(false);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        Init();

        if (property.managedReferenceValue == null)
            return EditorGUIUtility.singleLineHeight;

        var concreteType = property.managedReferenceValue.GetType();

        // Custom drawer height?
        if (_customDrawers.TryGetValue(concreteType, out var drawer))
            return EditorGUIUtility.singleLineHeight + 2 + drawer.GetHeight(property, label);


        // Fallback => default height
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

        // Dropdown to select concrete type
        Type currentType = property.managedReferenceValue?.GetType();
        int currentIndex = Array.IndexOf(_implTypes, currentType);
        int newIndex = EditorGUI.Popup(typeRect, label.text, currentIndex, _implNames);

        if (newIndex != currentIndex)
            property.managedReferenceValue = Activator.CreateInstance(_implTypes[newIndex]);

        if (property.managedReferenceValue != null)
        {
            var concreteType = property.managedReferenceValue.GetType();

            if (_customDrawers.TryGetValue(concreteType, out var drawer))
            {
                drawer.Draw(property, valueRect, label);
            }
            else
            {
                DrawDefault(property, valueRect);
            }
        }

        EditorGUI.EndProperty();
    }
}
