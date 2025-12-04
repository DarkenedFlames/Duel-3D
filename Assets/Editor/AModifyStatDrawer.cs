using UnityEditor;
using UnityEngine;

[ActionDrawer(typeof(AModifyStat))]
public class AModifyStatDrawer : IActionTypeDrawer
{
    const float VSpace = 2f;

    public float GetHeight(SerializedProperty property, GUIContent label)
    {
        float height = 0f;

        SerializedProperty modeProp = property.FindPropertyRelative("mode");
        SerializedProperty targetProp = property.FindPropertyRelative("target");
        SerializedProperty statDefinitionProp = property.FindPropertyRelative("StatDefinition");
        SerializedProperty modifierTargetProp = property.FindPropertyRelative("modifierTarget");
        SerializedProperty typeProp = property.FindPropertyRelative("type");
        SerializedProperty amountProp = property.FindPropertyRelative("amount");

        ModifyStatMode enabledMode = (ModifyStatMode)modeProp.enumValueIndex;
        ModifyStatTarget enabledTarget = (ModifyStatTarget)targetProp.enumValueIndex;
        StatModifierTarget enabledModifierTarget = (StatModifierTarget)modifierTargetProp.enumValueIndex;

        height += EditorGUI.GetPropertyHeight(modeProp, true) + VSpace;
        
        if (enabledMode == ModifyStatMode.AddModifier)
        {
            height += EditorGUI.GetPropertyHeight(statDefinitionProp) + VSpace;
            height += EditorGUI.GetPropertyHeight(typeProp) + VSpace;
            height += EditorGUI.GetPropertyHeight(amountProp) + VSpace;
        }
        else
        {
            height += EditorGUI.GetPropertyHeight(targetProp) + VSpace;
            
            if (enabledTarget == ModifyStatTarget.Specific)
                height += EditorGUI.GetPropertyHeight(statDefinitionProp) + VSpace;
            
            height += EditorGUI.GetPropertyHeight(targetProp) + VSpace;

            if (enabledModifierTarget == StatModifierTarget.Specific || enabledModifierTarget == StatModifierTarget.SpecificFromSource)
            {
                height += EditorGUI.GetPropertyHeight(typeProp) + VSpace;
                height += EditorGUI.GetPropertyHeight(amountProp) + VSpace;
            }
        }
       
        return height;
    }

    public void Draw(SerializedProperty property, Rect position, GUIContent label)
    {
        float y = position.y;

        // Get properties
        SerializedProperty modeProp = property.FindPropertyRelative("mode");
        SerializedProperty targetProp = property.FindPropertyRelative("target");
        SerializedProperty statDefinitionProp = property.FindPropertyRelative("StatDefinition");
        SerializedProperty modifierTargetProp = property.FindPropertyRelative("modifierTarget");
        SerializedProperty typeProp = property.FindPropertyRelative("type");
        SerializedProperty amountProp = property.FindPropertyRelative("amount");

        ModifyStatMode enabledMode = (ModifyStatMode)modeProp.enumValueIndex;
        ModifyStatTarget enabledTarget = (ModifyStatTarget)targetProp.enumValueIndex;
        StatModifierTarget enabledModifierTarget = (StatModifierTarget)modifierTargetProp.enumValueIndex;

        float h;

        h = EditorGUI.GetPropertyHeight(modeProp);
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), modeProp);
        y += h + 2;
       
        if (enabledMode == ModifyStatMode.AddModifier)
        {
            h = EditorGUI.GetPropertyHeight(statDefinitionProp);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), statDefinitionProp);
            y += h + 2;

            h = EditorGUI.GetPropertyHeight(typeProp);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), typeProp);
            y += h + 2;
            
            h = EditorGUI.GetPropertyHeight(amountProp);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), amountProp);
            y += h + 2;
        }
        else
        {
            h = EditorGUI.GetPropertyHeight(targetProp);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), targetProp);
            y += h + 2;

            if (enabledTarget == ModifyStatTarget.Specific)
            {
                h = EditorGUI.GetPropertyHeight(statDefinitionProp);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), statDefinitionProp);
                y += h + 2;
            }

            h = EditorGUI.GetPropertyHeight(modifierTargetProp);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), modifierTargetProp);
            y += h + 2;

            if (enabledModifierTarget == StatModifierTarget.Specific || enabledModifierTarget == StatModifierTarget.SpecificFromSource)
            {
                h = EditorGUI.GetPropertyHeight(typeProp);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), typeProp);
                y += h + 2;

                h = EditorGUI.GetPropertyHeight(amountProp);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), amountProp);
                y += h + 2;
            }
        }        
    }
}