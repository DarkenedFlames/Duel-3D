using UnityEditor;
using UnityEngine;

[ActionDrawer(typeof(AModifyResource))]
public class AModifyResourceDrawer : IActionTypeDrawer
{
    const float VSpace = 2f;

    public float GetHeight(SerializedProperty property, GUIContent label)
    {
        float height = 0f;

        void GetProperty(string name, out SerializedProperty prop) =>
            prop = property.FindPropertyRelative(name);

        void AddHeight(SerializedProperty prop) =>
            height += EditorGUI.GetPropertyHeight(prop, true) + VSpace;

        // Add Conditions field
        GetProperty("Conditions", out SerializedProperty conditionsProp);
        if (conditionsProp != null)
            height += PolymorphicActionDrawer.GetConditionsListHeightStatic(conditionsProp);

        GetProperty("targetMode", out SerializedProperty targetModeProp);
        GetProperty("mode", out SerializedProperty modeProp);
        GetProperty("targetResource", out SerializedProperty targetResourceProp);
        GetProperty("resourceType", out SerializedProperty resourceTypeProp);
        GetProperty("targetModifier", out SerializedProperty targetModifierProp);
        GetProperty("modifierType", out SerializedProperty modifierTypeProp);
        GetProperty("resetRegeneration", out SerializedProperty resetRegenProp);
        GetProperty("amount", out SerializedProperty amountProp);

        ModifyResourceMode enabledMode = (ModifyResourceMode)modeProp.enumValueIndex;
        ModifyResourceTarget enabledTarget = (ModifyResourceTarget)targetResourceProp.enumValueIndex;
        ResourceModifierTarget enabledModifierTarget = (ResourceModifierTarget)targetModifierProp.enumValueIndex;

        AddHeight(targetResourceProp);
        AddHeight(modeProp);
        
        switch(enabledMode)
        {
            case ModifyResourceMode.ChangeValue:
                AddHeight(resourceTypeProp);
                AddHeight(resetRegenProp);
                AddHeight(amountProp);
                break;
            case ModifyResourceMode.AddModifier:
                AddHeight(resourceTypeProp);
                AddHeight(modifierTypeProp);
                AddHeight(amountProp);
                break;
            case ModifyResourceMode.RemoveModifier:
                AddHeight(targetResourceProp);
                
                if (enabledTarget == ModifyResourceTarget.Specific)
                    AddHeight(resourceTypeProp);
                
                AddHeight(targetModifierProp);

                if (enabledModifierTarget == ResourceModifierTarget.Specific || enabledModifierTarget == ResourceModifierTarget.SpecificFromSource)
                {
                    AddHeight(modifierTypeProp);
                    AddHeight(amountProp);
                }
                break;
        }
        return height;
    }

    public void Draw(SerializedProperty property, Rect position, GUIContent label)
    {
        float y = position.y;

        void GetProperty(string name, out SerializedProperty prop) =>
            prop = property.FindPropertyRelative(name);

        void DrawField(SerializedProperty prop)
        {
            float h = EditorGUI.GetPropertyHeight(prop);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), prop);
            y += h + VSpace;
        }

        // Draw Conditions field
        GetProperty("Conditions", out SerializedProperty conditionsProp);
        if (conditionsProp != null)
        {
            float conditionsHeight = PolymorphicActionDrawer.GetConditionsListHeightStatic(conditionsProp);
            PolymorphicActionDrawer.DrawConditionsListStatic(new Rect(position.x, y, position.width, conditionsHeight), conditionsProp);
            y += conditionsHeight;
        }

        GetProperty("targetMode", out SerializedProperty targetModeProp);
        GetProperty("mode", out SerializedProperty modeProp);
        GetProperty("targetResource", out SerializedProperty targetResourceProp);
        GetProperty("resourceType", out SerializedProperty resourceTypeProp);
        GetProperty("targetModifier", out SerializedProperty targetModifierProp);
        GetProperty("modifierType", out SerializedProperty modifierTypeProp);
        GetProperty("resetRegeneration", out SerializedProperty resetRegenProp);
        GetProperty("amount", out SerializedProperty amountProp);

        ModifyResourceMode enabledMode = (ModifyResourceMode)modeProp.enumValueIndex;
        ModifyResourceTarget enabledTarget = (ModifyResourceTarget)targetResourceProp.enumValueIndex;
        ResourceModifierTarget enabledModifierTarget = (ResourceModifierTarget)targetModifierProp.enumValueIndex;

        DrawField(targetModeProp);
        DrawField(modeProp);
        
        switch(enabledMode)
        {
            case ModifyResourceMode.ChangeValue:
                DrawField(resourceTypeProp);
                DrawField(resetRegenProp);
                DrawField(amountProp);
                break;

            case ModifyResourceMode.AddModifier:
                DrawField(resourceTypeProp);
                DrawField(modifierTypeProp);
                DrawField(amountProp);
                break;

            case ModifyResourceMode.RemoveModifier:
                DrawField(targetResourceProp);
                
                if (enabledTarget == ModifyResourceTarget.Specific)
                    DrawField(resourceTypeProp);
                
                DrawField(targetModifierProp);

                if (enabledModifierTarget == ResourceModifierTarget.Specific || enabledModifierTarget == ResourceModifierTarget.SpecificFromSource)
                {
                    DrawField(modifierTypeProp);
                    DrawField(amountProp);
                }
                break;
        }
    }
}