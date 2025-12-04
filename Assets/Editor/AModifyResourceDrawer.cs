using UnityEditor;
using UnityEngine;

[ActionDrawer(typeof(AModifyResource))]
public class AModifyResourceDrawer : IActionTypeDrawer
{
    const float VSpace = 2f;

    public float GetHeight(SerializedProperty property, GUIContent label)
    {
        float height = 0f;

        SerializedProperty modeProp = property.FindPropertyRelative("mode");
        SerializedProperty targetProp = property.FindPropertyRelative("target");
        SerializedProperty resourceDefinitionProp = property.FindPropertyRelative("resourceDefinition");
        SerializedProperty modifierTargetProp = property.FindPropertyRelative("modifierTarget");
        SerializedProperty typeProp = property.FindPropertyRelative("type");
        SerializedProperty amountProp = property.FindPropertyRelative("amount");

        ModifyResourceMode enabledMode = (ModifyResourceMode)modeProp.enumValueIndex;
        ModifyResourceTarget enabledTarget = (ModifyResourceTarget)targetProp.enumValueIndex;
        ResourceModifierTarget enabledModifierTarget = (ResourceModifierTarget)modifierTargetProp.enumValueIndex;

        height += EditorGUI.GetPropertyHeight(modeProp, true) + VSpace;
        
        switch(enabledMode)
        {
            case ModifyResourceMode.ChangeValue:
                height += EditorGUI.GetPropertyHeight(resourceDefinitionProp) + VSpace;
                height += EditorGUI.GetPropertyHeight(amountProp) + VSpace;
                break;
            case ModifyResourceMode.AddModifier:
                height += EditorGUI.GetPropertyHeight(resourceDefinitionProp) + VSpace;
                height += EditorGUI.GetPropertyHeight(typeProp) + VSpace;
                height += EditorGUI.GetPropertyHeight(amountProp) + VSpace;
                break;
            case ModifyResourceMode.RemoveModifier:
                height += EditorGUI.GetPropertyHeight(targetProp) + VSpace;
                
                if (enabledTarget == ModifyResourceTarget.Specific)
                    height += EditorGUI.GetPropertyHeight(resourceDefinitionProp) + VSpace;
                
                height += EditorGUI.GetPropertyHeight(modifierTargetProp) + VSpace;

                if (enabledModifierTarget == ResourceModifierTarget.Specific || enabledModifierTarget == ResourceModifierTarget.SpecificFromSource)
                {
                    height += EditorGUI.GetPropertyHeight(typeProp) + VSpace;
                    height += EditorGUI.GetPropertyHeight(amountProp) + VSpace;
                }
                break;
        }
        return height;
    }

    public void Draw(SerializedProperty property, Rect position, GUIContent label)
    {
        float y = position.y;

        SerializedProperty modeProp = property.FindPropertyRelative("mode");
        SerializedProperty targetProp = property.FindPropertyRelative("target");
        SerializedProperty resourceDefinitionProp = property.FindPropertyRelative("resourceDefinition");
        SerializedProperty modifierTargetProp = property.FindPropertyRelative("modifierTarget");
        SerializedProperty typeProp = property.FindPropertyRelative("type");
        SerializedProperty amountProp = property.FindPropertyRelative("amount");

        ModifyResourceMode enabledMode = (ModifyResourceMode)modeProp.enumValueIndex;
        ModifyResourceTarget enabledTarget = (ModifyResourceTarget)targetProp.enumValueIndex;
        ResourceModifierTarget enabledModifierTarget = (ResourceModifierTarget)modifierTargetProp.enumValueIndex;

        float h;

        h = EditorGUI.GetPropertyHeight(modeProp);
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), modeProp);
        y += h + 2;
        
        switch(enabledMode)
        {
            case ModifyResourceMode.ChangeValue:
                h = EditorGUI.GetPropertyHeight(resourceDefinitionProp);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), resourceDefinitionProp);
                y += h + 2;

                h = EditorGUI.GetPropertyHeight(amountProp);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), amountProp);
                y += h + 2;
                break;

            case ModifyResourceMode.AddModifier:
                h = EditorGUI.GetPropertyHeight(resourceDefinitionProp);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), resourceDefinitionProp);
                y += h + 2;

                h = EditorGUI.GetPropertyHeight(typeProp);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), typeProp);
                y += h + 2;

                h = EditorGUI.GetPropertyHeight(amountProp);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), amountProp);
                y += h + 2;
                break;

            case ModifyResourceMode.RemoveModifier:
                h = EditorGUI.GetPropertyHeight(targetProp);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), targetProp);
                y += h + 2;
                
                if (enabledTarget == ModifyResourceTarget.Specific)
                {
                    h = EditorGUI.GetPropertyHeight(resourceDefinitionProp);
                    EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), resourceDefinitionProp);
                    y += h + 2;
                }
             
                h = EditorGUI.GetPropertyHeight(modifierTargetProp);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), modifierTargetProp);
                y += h + 2;

                if (enabledModifierTarget == ResourceModifierTarget.Specific || enabledModifierTarget == ResourceModifierTarget.SpecificFromSource)
                {
                    h = EditorGUI.GetPropertyHeight(typeProp);
                    EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), typeProp);
                    y += h + 2;

                    h = EditorGUI.GetPropertyHeight(amountProp);
                    EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), amountProp);
                    y += h + 2;
                }
                break;
        }
    }
}