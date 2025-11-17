#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

[CustomPropertyDrawer(typeof(EventReaction), true)]
public class EventReactionDrawer : PropertyDrawer
{
    private bool foldout = true;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Get the current object (instance of subclass)
        object obj = fieldInfo.GetValue(property.serializedObject.targetObject);

        // Draw type selector dropdown
        Type[] types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && typeof(EventReaction).IsAssignableFrom(t))
            .ToArray();

        int currentIndex = Array.FindIndex(types, t => obj != null && t == obj.GetType());
        string[] typeNames = types.Select(t => t.Name).ToArray();

        int selected = EditorGUI.Popup(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            "Event Reaction Type", currentIndex, typeNames);

        if (selected != currentIndex)
        {
            // Create new instance of selected type
            obj = Activator.CreateInstance(types[selected]);
            property.managedReferenceValue = obj;
        }

        // Draw the fields of the instance
        if (obj != null)
        {
            foldout = EditorGUI.Foldout(new Rect(position.x, position.y + 18, position.width, EditorGUIUtility.singleLineHeight),
                                      foldout, "Settings"/*obj.GetType().Name*/, true);

            if (foldout)
            {
                EditorGUI.indentLevel++;
                SerializedProperty sp = property.Copy();
                SerializedProperty end = sp.GetEndProperty(true);

                sp.NextVisible(true); // skip managedReference field itself
                float y = position.y + 36;
                while (!SerializedProperty.EqualContents(sp, end))
                {
                    float h = EditorGUI.GetPropertyHeight(sp, true);
                    EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), sp, true);
                    y += h + 2;
                    sp.NextVisible(false);
                }

                EditorGUI.indentLevel--;
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        object obj = property.managedReferenceValue;
        if (obj == null) return EditorGUIUtility.singleLineHeight + 2;
        float height = EditorGUIUtility.singleLineHeight + 18;
        if (foldout)
        {
            SerializedProperty sp = property.Copy();
            SerializedProperty end = sp.GetEndProperty(true);
            sp.NextVisible(true);

            while (!SerializedProperty.EqualContents(sp, end))
            {
                height += EditorGUI.GetPropertyHeight(sp, true) + 2;
                sp.NextVisible(false);
            }
        }
        return height;
    }
}
#endif
