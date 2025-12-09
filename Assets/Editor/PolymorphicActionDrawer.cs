using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

// Creates a dropdown wherever IGameAction is serialized
// Creates a new instance once selected
// Uses the custom drawer for the type
// If no custom drawer, draws all fields of the type

[CustomPropertyDrawer(typeof(IGameAction), true)]
public class PolymorphicActionDrawer : PropertyDrawer
{
    static Type[] _types;
    static string[] _names;
    static Dictionary<Type, IActionTypeDrawer> _customDrawers;

    void Init()
    {
        if (_types != null) return;

        // find all non-abstract IGameAction implementations
        _types = AppDomain.CurrentDomain.GetAssemblies()
               .SelectMany(a => a.GetTypes())
               .Where(t => typeof(IGameAction).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
               .ToArray();

        _names = _types.Select(t => t.Name).ToArray();

        // find custom drawers
        _customDrawers =
            AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IActionTypeDrawer).IsAssignableFrom(t) && !t.IsAbstract)
            .Select(t => new {
                Drawer = t,
                Attr = t.GetCustomAttribute<ActionDrawerAttribute>()
            })
            .Where(x => x.Attr != null)
            .ToDictionary(
                x => x.Attr.TargetType,
                x => (IActionTypeDrawer)Activator.CreateInstance(x.Drawer)
            );
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        Init();

        var obj = property.managedReferenceValue;

        // If null — only the dropdown is drawn
        if (obj == null)
            return EditorGUIUtility.singleLineHeight;

        var type = obj.GetType();

        float height = EditorGUIUtility.singleLineHeight + 4;

        if (_customDrawers.TryGetValue(type, out var drawer))
            return height + drawer.GetHeight(property, label);

        // default: draw all fields
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

        // type selection popup
        Rect popupRect = new(pos.x, y, pos.width, EditorGUIUtility.singleLineHeight);

        int currentIndex = (obj == null)
            ? -1
            : Array.IndexOf(_types, obj.GetType());

        int newIndex = EditorGUI.Popup(popupRect, "Action Type", currentIndex, _names);
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

        var type = obj.GetType();
        Rect body = new(pos.x, y, pos.width, pos.height - (y - pos.y));

        if (_customDrawers.TryGetValue(type, out var drawer))
            drawer.Draw(property, body, label);
        else
            DrawDefault(body, property, label);

        EditorGUI.EndProperty();
    }

    void DrawDefault(Rect pos, SerializedProperty property, GUIContent label)
    {
        var copy = property.Copy();
        var end = copy.GetEndProperty();

        copy.NextVisible(true);

        float y = pos.y;

        while (!SerializedProperty.EqualContents(copy, end))
        {
            float h = EditorGUI.GetPropertyHeight(copy, true);
            EditorGUI.PropertyField(new Rect(pos.x, y, pos.width, h), copy, true);
            y += h + 2;

            copy.NextVisible(false);
        }
    }
}
