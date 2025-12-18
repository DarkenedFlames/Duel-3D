using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[ActionDrawer(typeof(AModifyStat))]
public class AModifyStatDrawer : IActionTypeDrawer
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
        GetProperty("statDefinition", out SerializedProperty statDefinitionProp);
        GetProperty("statDefinitions", out SerializedProperty statDefinitionsProp);
        GetProperty("targetModifierType", out SerializedProperty targetModifierTypeProp);
        GetProperty("amount", out SerializedProperty amountProp);
        GetProperty("numberIconUI", out SerializedProperty numberIconUIProp);

        AModifyStat.Mode mode = (AModifyStat.Mode)modeProp.enumValueIndex;

        AddHeight(targetModeProp);
        AddHeight(modeProp);

        // Determine which fields to show based on mode
        switch (mode)
        {
            case AModifyStat.Mode.AddModifierToSpecificStat:
                AddHeight(statDefinitionProp);
                AddHeight(targetModifierTypeProp);
                AddHeight(amountProp);
                break;
            case AModifyStat.Mode.AddModifierToRandomStatFromSet:
            case AModifyStat.Mode.AddModifierToAllStatsFromSet:
                AddHeight(statDefinitionsProp);
                AddHeight(targetModifierTypeProp);
                AddHeight(amountProp);
                break;
            case AModifyStat.Mode.AddModifierToAllStats:
                AddHeight(targetModifierTypeProp);
                AddHeight(amountProp);
                break;
            case AModifyStat.Mode.RemoveSpecificModifierFromSpecificStat:
                AddHeight(removeOnlyFromSourceProp);
                AddHeight(statDefinitionProp);
                AddHeight(targetModifierTypeProp);
                AddHeight(amountProp);
                break;
            case AModifyStat.Mode.RemoveSpecificModifierFromAllStats:
                AddHeight(removeOnlyFromSourceProp);
                AddHeight(targetModifierTypeProp);
                AddHeight(amountProp);
                break;
            case AModifyStat.Mode.RemoveAllModifiersFromSpecificStat:
                AddHeight(removeOnlyFromSourceProp);
                AddHeight(statDefinitionProp);
                break;
            case AModifyStat.Mode.RemoveAllModifiersFromAllStats:
                AddHeight(removeOnlyFromSourceProp);
                break;
        }
        AddHeight(numberIconUIProp);

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
        GetProperty("statDefinition", out SerializedProperty statDefinitionProp);
        GetProperty("statDefinitions", out SerializedProperty statDefinitionsProp);
        GetProperty("targetModifierType", out SerializedProperty targetModifierTypeProp);
        GetProperty("amount", out SerializedProperty amountProp);
        GetProperty("numberIconUI", out SerializedProperty numberIconUIProp);

        AModifyStat.Mode mode = (AModifyStat.Mode)modeProp.enumValueIndex;

        DrawField(targetModeProp);
        DrawField(modeProp);

        // Draw only the relevant fields based on mode
        switch (mode)
        {
            case AModifyStat.Mode.AddModifierToSpecificStat:
                DrawField(statDefinitionProp);
                DrawField(targetModifierTypeProp);
                DrawField(amountProp);
                break;
            case AModifyStat.Mode.AddModifierToRandomStatFromSet:
            case AModifyStat.Mode.AddModifierToAllStatsFromSet:
                DrawField(statDefinitionsProp);
                DrawField(targetModifierTypeProp);
                DrawField(amountProp);
                break;
            case AModifyStat.Mode.AddModifierToAllStats:
                DrawField(targetModifierTypeProp);
                DrawField(amountProp);
                break;
            case AModifyStat.Mode.RemoveSpecificModifierFromSpecificStat:
                DrawField(removeOnlyFromSourceProp);
                DrawField(statDefinitionProp);
                DrawField(targetModifierTypeProp);
                DrawField(amountProp);
                break;
            case AModifyStat.Mode.RemoveSpecificModifierFromAllStats:
                DrawField(removeOnlyFromSourceProp);
                DrawField(targetModifierTypeProp);
                DrawField(amountProp);
                break;
            case AModifyStat.Mode.RemoveAllModifiersFromSpecificStat:
                DrawField(removeOnlyFromSourceProp);
                DrawField(statDefinitionProp);
                break;
            case AModifyStat.Mode.RemoveAllModifiersFromAllStats:
                DrawField(removeOnlyFromSourceProp);
                break;
        }
        DrawField(numberIconUIProp); 
    }
}