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

        _names = _types.Select(t => FormatActionName(t.Name)).ToArray();

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

        // default: draw all fields including Conditions list
        var copy = property.Copy();
        var end = copy.GetEndProperty();
        copy.NextVisible(true);

        while (!SerializedProperty.EqualContents(copy, end))
        {
            // Special handling for Conditions list
            if (copy.name == "Conditions")
            {
                height += GetConditionsListHeight(copy);
            }
            else
            {
                height += EditorGUI.GetPropertyHeight(copy, true) + 2;
            }
            copy.NextVisible(false);
        }

        return height;
    }

    float GetConditionsListHeight(SerializedProperty conditionsProperty)
    {
        return PolymorphicActionDrawer.GetConditionsListHeightStatic(conditionsProperty);
    }

    public static float GetConditionsListHeightStatic(SerializedProperty conditionsProperty)
    {
        if (conditionsProperty == null || !conditionsProperty.isArray)
            return EditorGUIUtility.singleLineHeight;

        float height = EditorGUIUtility.singleLineHeight + 4; // "Conditions" header + Add button

        for (int i = 0; i < conditionsProperty.arraySize; i++)
        {
            var element = conditionsProperty.GetArrayElementAtIndex(i);
            height += EditorGUIUtility.singleLineHeight + 4; // Condition header with buttons
            height += EditorGUI.GetPropertyHeight(element, true) + 4; // Condition content
            height += 5; // Spacing
        }

        height += EditorGUIUtility.singleLineHeight + 2; // Add button

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
            // Special handling for Conditions list
            if (copy.name == "Conditions")
            {
                DrawConditionsList(new Rect(pos.x, y, pos.width, GetConditionsListHeight(copy)), copy);
                y += GetConditionsListHeight(copy);
            }
            else
            {
                float h = EditorGUI.GetPropertyHeight(copy, true);
                EditorGUI.PropertyField(new Rect(pos.x, y, pos.width, h), copy, true);
                y += h + 2;
            }

            copy.NextVisible(false);
        }
    }

    void DrawConditionsList(Rect pos, SerializedProperty listProperty)
    {
        PolymorphicActionDrawer.DrawConditionsListStatic(pos, listProperty);
    }

    public static void DrawConditionsListStatic(Rect pos, SerializedProperty listProperty)
    {
        float y = pos.y;

        // Draw "Conditions" header
        EditorGUI.LabelField(new Rect(pos.x, y, pos.width, EditorGUIUtility.singleLineHeight), "Conditions", EditorStyles.boldLabel);
        y += EditorGUIUtility.singleLineHeight + 4;

        // Draw each condition
        for (int i = 0; i < listProperty.arraySize; i++)
        {
            EditorGUI.BeginChangeCheck();
            
            var element = listProperty.GetArrayElementAtIndex(i);
            
            // Draw header with buttons
            Rect headerRect = new Rect(pos.x, y, pos.width, EditorGUIUtility.singleLineHeight);
            Rect labelRect = new Rect(pos.x, y, pos.width - 145, EditorGUIUtility.singleLineHeight);
            
            EditorGUI.LabelField(labelRect, $"Condition #{i + 1}", EditorStyles.boldLabel);

            // Move up button
            Rect upRect = new Rect(pos.x + pos.width - 145, y, 22, EditorGUIUtility.singleLineHeight);
            GUI.enabled = i > 0;
            if (GUI.Button(upRect, "▲"))
                listProperty.MoveArrayElement(i, i - 1);
            GUI.enabled = true;

            // Move down button
            Rect downRect = new Rect(pos.x + pos.width - 121, y, 22, EditorGUIUtility.singleLineHeight);
            GUI.enabled = i < listProperty.arraySize - 1;
            if (GUI.Button(downRect, "▼"))
                listProperty.MoveArrayElement(i, i + 1);
            GUI.enabled = true;

            // Duplicate button
            Rect dupRect = new Rect(pos.x + pos.width - 97, y, 70, EditorGUIUtility.singleLineHeight);
            if (GUI.Button(dupRect, "Duplicate"))
            {
                DuplicateCondition(listProperty, i);
            }

            // Delete button
            Rect delRect = new Rect(pos.x + pos.width - 25, y, 25, EditorGUIUtility.singleLineHeight);
            if (GUI.Button(delRect, "X"))
            {
                listProperty.DeleteArrayElementAtIndex(i);
                if (EditorGUI.EndChangeCheck())
                    listProperty.serializedObject.ApplyModifiedProperties();
                return;
            }

            y += EditorGUIUtility.singleLineHeight + 4;

            // Draw the condition content
            float conditionHeight = EditorGUI.GetPropertyHeight(element, true);
            EditorGUI.PropertyField(new Rect(pos.x, y, pos.width, conditionHeight), element, GUIContent.none, true);
            y += conditionHeight + 5;

            if (EditorGUI.EndChangeCheck())
                listProperty.serializedObject.ApplyModifiedProperties();
        }

        // Add new condition button
        Rect addRect = new Rect(pos.x, y, pos.width, EditorGUIUtility.singleLineHeight);
        if (GUI.Button(addRect, "Add New Condition"))
        {
            listProperty.arraySize++;
            var newElement = listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);
            newElement.managedReferenceValue = null;
            listProperty.serializedObject.ApplyModifiedProperties();
        }
    }

    static void DuplicateCondition(SerializedProperty listProperty, int index)
    {
        var sourceElement = listProperty.GetArrayElementAtIndex(index);
        
        listProperty.InsertArrayElementAtIndex(index);
        var newElement = listProperty.GetArrayElementAtIndex(index + 1);
        
        // Deep copy the managed reference
        if (sourceElement.managedReferenceValue != null)
        {
            var sourceJson = JsonUtility.ToJson(sourceElement.managedReferenceValue);
            var sourceType = sourceElement.managedReferenceValue.GetType();
            var newInstance = Activator.CreateInstance(sourceType);
            JsonUtility.FromJsonOverwrite(sourceJson, newInstance);
            newElement.managedReferenceValue = newInstance;
        }
        
        listProperty.serializedObject.ApplyModifiedProperties();
    }

    static string FormatActionName(string typeName)
    {
        // Remove leading 'A' if present
        if (typeName.StartsWith("A") && typeName.Length > 1 && char.IsUpper(typeName[1]))
            typeName = typeName[1..];

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
