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
        GetProperty("removeOnlyFromSource", out SerializedProperty removeOnlyFromSourceProp);
        GetProperty("resourceDefinition", out SerializedProperty resourceDefinitionProp);
        GetProperty("resourceDefinitions", out SerializedProperty resourceDefinitionsProp);
        GetProperty("targetModifierType", out SerializedProperty targetModifierTypeProp);
        GetProperty("resetRegeneration", out SerializedProperty resetRegenerationProp);
        GetProperty("amount", out SerializedProperty amountProp);

        AModifyResource.Mode mode = (AModifyResource.Mode)modeProp.enumValueIndex;

        AddHeight(targetModeProp);
        AddHeight(modeProp);

        // Determine which fields to show based on mode
        switch (mode)
        {
            case AModifyResource.Mode.ChangeSpecificResourceValue:
                AddHeight(resourceDefinitionProp);
                AddHeight(amountProp);
                AddHeight(resetRegenerationProp);
                break;

            case AModifyResource.Mode.AddModifierToSpecificResource:
                AddHeight(resourceDefinitionProp);
                AddHeight(targetModifierTypeProp);
                AddHeight(amountProp);
                break;
            case AModifyResource.Mode.AddModifierToRandomResourceFromSet:
            case AModifyResource.Mode.AddModifierToAllResourcesFromSet:
                AddHeight(resourceDefinitionsProp);
                AddHeight(targetModifierTypeProp);
                AddHeight(amountProp);
                break;
            case AModifyResource.Mode.AddModifierToAllResources:
                AddHeight(targetModifierTypeProp);
                AddHeight(amountProp);
                break;
            case AModifyResource.Mode.RemoveSpecificModifierFromSpecificResource:
                AddHeight(removeOnlyFromSourceProp);
                AddHeight(resourceDefinitionProp);
                AddHeight(targetModifierTypeProp);
                AddHeight(amountProp);
                break;
            case AModifyResource.Mode.RemoveSpecificModifierFromAllResources:
                AddHeight(removeOnlyFromSourceProp);
                AddHeight(targetModifierTypeProp);
                AddHeight(amountProp);
                break;
            case AModifyResource.Mode.RemoveAllModifiersFromSpecificResource:
                AddHeight(removeOnlyFromSourceProp);
                AddHeight(resourceDefinitionProp);
                break;
            case AModifyResource.Mode.RemoveAllModifiersFromAllResources:
                AddHeight(removeOnlyFromSourceProp);
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
            y += h + 2;
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
        GetProperty("removeOnlyFromSource", out SerializedProperty removeOnlyFromSourceProp);
        GetProperty("resourceDefinition", out SerializedProperty resourceDefinitionProp);
        GetProperty("resourceDefinitions", out SerializedProperty resourceDefinitionsProp);
        GetProperty("targetModifierType", out SerializedProperty targetModifierTypeProp);
        GetProperty("resetRegeneration", out SerializedProperty resetRegenerationProp);
        GetProperty("amount", out SerializedProperty amountProp);

        AModifyResource.Mode mode = (AModifyResource.Mode)modeProp.enumValueIndex;

        DrawField(targetModeProp);
        DrawField(modeProp);

        // Determine which fields to show based on mode
        switch (mode)
        {
            case AModifyResource.Mode.ChangeSpecificResourceValue:
                DrawField(resourceDefinitionProp);
                DrawField(amountProp);
                DrawField(resetRegenerationProp);
                break;

            case AModifyResource.Mode.AddModifierToSpecificResource:
                DrawField(resourceDefinitionProp);
                DrawField(targetModifierTypeProp);
                DrawField(amountProp);
                break;
            case AModifyResource.Mode.AddModifierToRandomResourceFromSet:
            case AModifyResource.Mode.AddModifierToAllResourcesFromSet:
                DrawField(resourceDefinitionsProp);
                DrawField(targetModifierTypeProp);
                DrawField(amountProp);
                break;
            case AModifyResource.Mode.AddModifierToAllResources:
                DrawField(targetModifierTypeProp);
                DrawField(amountProp);
                break;
            case AModifyResource.Mode.RemoveSpecificModifierFromSpecificResource:
                DrawField(removeOnlyFromSourceProp);
                DrawField(resourceDefinitionProp);
                DrawField(targetModifierTypeProp);
                DrawField(amountProp);
                break;
            case AModifyResource.Mode.RemoveSpecificModifierFromAllResources:
                DrawField(removeOnlyFromSourceProp);
                DrawField(targetModifierTypeProp);
                DrawField(amountProp);
                break;
            case AModifyResource.Mode.RemoveAllModifiersFromSpecificResource:
                DrawField(removeOnlyFromSourceProp);
                DrawField(resourceDefinitionProp);
                break;
            case AModifyResource.Mode.RemoveAllModifiersFromAllResources:
                DrawField(removeOnlyFromSourceProp);
                break;
        }        
    }
}