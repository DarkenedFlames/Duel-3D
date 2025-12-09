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

        GetProperty("targetMode", out SerializedProperty targetModeProp);
        GetProperty("mode", out SerializedProperty modeProp);
        GetProperty("targetStat", out SerializedProperty targetStatProp);
        GetProperty("statType", out SerializedProperty statTypeProp);
        GetProperty("targetModifier", out SerializedProperty targetModifierProp);
        GetProperty("targetModifierType", out SerializedProperty modifierTypeProp);
        GetProperty("amount", out SerializedProperty amountProp);

        ModifyStatMode enabledMode = (ModifyStatMode)modeProp.enumValueIndex;
        ModifyStatTarget enabledTarget = (ModifyStatTarget)targetStatProp.enumValueIndex;
        StatModifierTarget enabledModifierTarget = (StatModifierTarget)targetModifierProp.enumValueIndex;

        AddHeight(targetModeProp);
        AddHeight(modeProp);
        
        if (enabledMode == ModifyStatMode.AddModifier)
        {
            AddHeight(statTypeProp);
            AddHeight(modifierTypeProp);
            AddHeight(amountProp);
        }
        else
        {
            AddHeight(targetStatProp);
            
            if (enabledTarget == ModifyStatTarget.Specific)
                AddHeight(statTypeProp);
            
            AddHeight(targetModifierProp);

            if (enabledModifierTarget == StatModifierTarget.Specific || enabledModifierTarget == StatModifierTarget.SpecificFromSource)
            {
                AddHeight(modifierTypeProp);
                AddHeight(amountProp);
            }
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

        GetProperty("targetMode", out SerializedProperty targetModeProp);
        GetProperty("mode", out SerializedProperty modeProp);
        GetProperty("targetStat", out SerializedProperty targetStatProp);
        GetProperty("statType", out SerializedProperty statTypeProp);
        GetProperty("targetModifier", out SerializedProperty targetModifierProp);
        GetProperty("targetModifierType", out SerializedProperty modifierTypeProp);
        GetProperty("amount", out SerializedProperty amountProp);

        ModifyStatMode enabledMode = (ModifyStatMode)modeProp.enumValueIndex;
        ModifyStatTarget enabledTarget = (ModifyStatTarget)targetStatProp.enumValueIndex;
        StatModifierTarget enabledModifierTarget = (StatModifierTarget)targetModifierProp.enumValueIndex;

        DrawField(targetModeProp);
        DrawField(modeProp);
       
        if (enabledMode == ModifyStatMode.AddModifier)
        {
            DrawField(statTypeProp);
            DrawField(modifierTypeProp);
            DrawField(amountProp);
        }
        else
        {
            DrawField(targetStatProp);

            if (enabledTarget == ModifyStatTarget.Specific)
                DrawField(statTypeProp);

            DrawField(targetModifierProp);

            if (enabledModifierTarget == StatModifierTarget.Specific || enabledModifierTarget == StatModifierTarget.SpecificFromSource)
            {
                DrawField(modifierTypeProp);
                DrawField(amountProp);
            }
        }        
    }
}