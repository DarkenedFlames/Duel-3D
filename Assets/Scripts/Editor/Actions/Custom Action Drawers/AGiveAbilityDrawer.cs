using UnityEditor;
using UnityEngine;

[ActionDrawer(typeof(AGiveAbility))]
public class AGiveAbilityDrawer : IActionTypeDrawer
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
        GetProperty("abilityDefinition", out SerializedProperty abilityDefinitionProp);
        GetProperty("set", out SerializedProperty setProp);
        GetProperty("family", out SerializedProperty familyProp);

        AddHeight(targetModeProp);
        AddHeight(modeProp);

        switch ((GiveAbilityMode)modeProp.enumValueIndex)
        {
            case GiveAbilityMode.Specific: AddHeight(abilityDefinitionProp); break;
            case GiveAbilityMode.RandomBySlotFromSet: AddHeight(setProp); break;
            case GiveAbilityMode.RandomByFamilyFromSet:
                AddHeight(setProp);
                AddHeight(familyProp);
                break;
        }
        
        return height;
    }

    public void Draw(SerializedProperty property, Rect position, GUIContent label)
    {
        float y = position.y;

        void DrawField(SerializedProperty prop)
        {
            float h = EditorGUI.GetPropertyHeight(prop);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), prop);
            y += h + VSpace;
        }

        void GetProperty(string name, out SerializedProperty prop) =>
            prop = property.FindPropertyRelative(name);

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
        GetProperty("abilityDefinition", out SerializedProperty abilityDefinitionProp);
        GetProperty("set", out SerializedProperty setProp);
        GetProperty("family", out SerializedProperty familyProp);

        DrawField(targetModeProp);
        DrawField(modeProp);
       
        switch ((GiveAbilityMode)modeProp.enumValueIndex)
        {
            case GiveAbilityMode.Specific: DrawField(abilityDefinitionProp); break;
            case GiveAbilityMode.RandomBySlotFromSet: DrawField(setProp); break;
            case GiveAbilityMode.RandomByFamilyFromSet:
                DrawField(setProp);
                DrawField(familyProp);
                break;
        }
    }
}