using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EffectDefinition))]
public class EffectDefinitionEditor : Editor
{
    private SerializedProperty actionsProperty;

    private void OnEnable()
    {
        actionsProperty = serializedObject.FindProperty("Actions");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "Actions");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
        
        DrawActionsList(actionsProperty);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawActionsList(SerializedProperty listProperty)
    {
        EditorGUILayout.PropertyField(listProperty.FindPropertyRelative("Array.size"));

        for (int i = 0; i < listProperty.arraySize; i++)
        {
            EditorGUILayout.BeginVertical("box");
            
            var element = listProperty.GetArrayElementAtIndex(i);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Action #{i+1}", EditorStyles.boldLabel);

            GUI.enabled = i > 0;
            if (GUILayout.Button("▲", GUILayout.Width(22)))
                listProperty.MoveArrayElement(i, i - 1);
            GUI.enabled = true;

            GUI.enabled = i < listProperty.arraySize - 1;
            if (GUILayout.Button("▼", GUILayout.Width(22)))
                listProperty.MoveArrayElement(i, i + 1);
            GUI.enabled = true;
            
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
        
        var sourceHook = sourceElement.FindPropertyRelative("Hook");
        var newHook = newElement.FindPropertyRelative("Hook");
        if (sourceHook != null && newHook != null)
        {
            newHook.enumValueIndex = sourceHook.enumValueIndex;
        }
        
        var sourceAction = sourceElement.FindPropertyRelative("Action");
        var newAction = newElement.FindPropertyRelative("Action");
        
        if (sourceAction != null && newAction != null && sourceAction.managedReferenceValue != null)
        {
            var sourceObj = sourceAction.managedReferenceValue;
            var type = sourceObj.GetType();
            var copy = System.Activator.CreateInstance(type);
            
            var flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
            foreach (var field in type.GetFields(flags))
            {
                var value = field.GetValue(sourceObj);
                
                // Deep copy the Conditions list
                if (field.Name == "Conditions" && value is System.Collections.IList sourceList)
                {
                    var listType = field.FieldType;
                    var newList = (System.Collections.IList)System.Activator.CreateInstance(listType);
                    
                    foreach (var item in sourceList)
                    {
                        if (item != null)
                        {
                            var itemType = item.GetType();
                            var itemCopy = System.Activator.CreateInstance(itemType);
                            
                            // Copy all fields of the condition
                            foreach (var itemField in itemType.GetFields(flags))
                            {
                                itemField.SetValue(itemCopy, itemField.GetValue(item));
                            }
                            
                            newList.Add(itemCopy);
                        }
                    }
                    
                    field.SetValue(copy, newList);
                }
                else
                {
                    field.SetValue(copy, value);
                }
            }
            
            newAction.managedReferenceValue = copy;
        }
    }
}
