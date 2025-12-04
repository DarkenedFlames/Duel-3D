using UnityEditor;
using UnityEngine;

[ActionDrawer(typeof(AGiveAbility))]
public class AGiveAbilityDrawer : IActionTypeDrawer
{
    const float VSpace = 2f;

    public float GetHeight(SerializedProperty property, GUIContent label)
    {
        float height = 0f;

        SerializedProperty modeProp = property.FindPropertyRelative("mode");
        SerializedProperty abilityDefinitionProp = property.FindPropertyRelative("abilityDefinition");
        SerializedProperty setProp = property.FindPropertyRelative("set");

        GiveAbilityMode enabledMode = (GiveAbilityMode)modeProp.enumValueIndex;

        height += EditorGUI.GetPropertyHeight(modeProp, true) + VSpace;

        switch (enabledMode)
        {
            case GiveAbilityMode.Specific: height += EditorGUI.GetPropertyHeight(abilityDefinitionProp) + VSpace; break;
            case GiveAbilityMode.RandomBySlotFromSet: height += EditorGUI.GetPropertyHeight(setProp) + VSpace; break;
        }
        
        return height;
    }

    public void Draw(SerializedProperty property, Rect position, GUIContent label)
    {
        float y = position.y;
        float h;

        SerializedProperty modeProp = property.FindPropertyRelative("mode");
        SerializedProperty abilityDefinitionProp = property.FindPropertyRelative("abilityDefinition");
        SerializedProperty setProp = property.FindPropertyRelative("set");

        h = EditorGUI.GetPropertyHeight(modeProp);
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), modeProp);
        y += h + 2;

        GiveAbilityMode enabledMode = (GiveAbilityMode)modeProp.enumValueIndex;
       
        switch (enabledMode)
        {
            case GiveAbilityMode.Specific: 
                h = EditorGUI.GetPropertyHeight(abilityDefinitionProp);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), abilityDefinitionProp);
                y += h + 2;
                break;

            case GiveAbilityMode.RandomBySlotFromSet:
                h = EditorGUI.GetPropertyHeight(setProp);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), setProp);
                y += h + 2;
                break;
        }
    }
}