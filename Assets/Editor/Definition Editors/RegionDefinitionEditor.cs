using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RegionDefinition))]
public class RegionDefinitionEditor : Editor
{
    private SerializedProperty actionsProperty;

    private void OnEnable()
    {
        actionsProperty = serializedObject.FindProperty("Actions");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw default inspector for everything except Actions
        DrawPropertiesExcluding(serializedObject, "Actions");

        // Draw Actions list with custom handling
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
        
        DrawActionsList(actionsProperty);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawActionsList(SerializedProperty listProperty)
    {
        // Show array size control
        EditorGUILayout.PropertyField(listProperty.FindPropertyRelative("Array.size"));

        // Draw each element
        for (int i = 0; i < listProperty.arraySize; i++)
        {
            EditorGUILayout.BeginVertical("box");
            
            var element = listProperty.GetArrayElementAtIndex(i);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Element {i}", EditorStyles.boldLabel);
            
            // Add duplicate button with proper deep copy
            if (GUILayout.Button("Duplicate", GUILayout.Width(70)))
            {
                DuplicateElement(listProperty, i);
            }
            
            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                listProperty.DeleteArrayElementAtIndex(i);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                break;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.PropertyField(element, GUIContent.none, true);
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }

        // Add button
        if (GUILayout.Button("Add New Action Entry"))
        {
            listProperty.arraySize++;
            var newElement = listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);
            var actionProp = newElement.FindPropertyRelative("Action");
            if (actionProp != null)
            {
                actionProp.managedReferenceValue = null;
            }
        }
    }

    private void DuplicateElement(SerializedProperty listProperty, int index)
    {
        var sourceElement = listProperty.GetArrayElementAtIndex(index);
        
        listProperty.InsertArrayElementAtIndex(index);
        var newElement = listProperty.GetArrayElementAtIndex(index + 1);
        
        // Copy Hook value
        var sourceHook = sourceElement.FindPropertyRelative("Hook");
        var newHook = newElement.FindPropertyRelative("Hook");
        if (sourceHook != null && newHook != null)
        {
            newHook.enumValueIndex = sourceHook.enumValueIndex;
        }
        
        // Deep copy the Action
        var sourceAction = sourceElement.FindPropertyRelative("Action");
        var newAction = newElement.FindPropertyRelative("Action");
        
        if (sourceAction != null && newAction != null && sourceAction.managedReferenceValue != null)
        {
            var sourceObj = sourceAction.managedReferenceValue;
            var type = sourceObj.GetType();
            var copy = System.Activator.CreateInstance(type);
            
            // Copy all fields using reflection
            var flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
            foreach (var field in type.GetFields(flags))
            {
                field.SetValue(copy, field.GetValue(sourceObj));
            }
            
            newAction.managedReferenceValue = copy;
        }
    }
}
