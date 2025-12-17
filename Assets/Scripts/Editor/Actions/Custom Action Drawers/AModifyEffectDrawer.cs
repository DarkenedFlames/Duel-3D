using UnityEditor;
using UnityEngine;

[ActionDrawer(typeof(AModifyEffect))]
public class AModifyEffectDrawer : IActionTypeDrawer
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
        GetProperty("mode", out SerializedProperty modifyModeProp);
        GetProperty("effectToApply", out SerializedProperty effectToApplyProp);
        GetProperty("stacksToApply", out SerializedProperty stacksToApplyProp);
        GetProperty("removeMode", out SerializedProperty removeModeProp);
        GetProperty("removeTarget", out SerializedProperty removeTargetProp);
        GetProperty("effectToRemove", out SerializedProperty effectToRemoveProp);
        GetProperty("stacksToRemove", out SerializedProperty stacksToRemoveProp);

        EffectModifyMode enabledMode = (EffectModifyMode)modifyModeProp.enumValueIndex;
        EffectRemoveMode enabledRemoveMode = (EffectRemoveMode)removeModeProp.enumValueIndex;
        EffectRemoveTarget enabledTarget = (EffectRemoveTarget)removeTargetProp.enumValueIndex;

        AddHeight(targetModeProp);
        AddHeight(modifyModeProp);
        
        if (enabledMode == EffectModifyMode.Apply)
        {
            AddHeight(effectToApplyProp);
            AddHeight(stacksToApplyProp);
        }
        else
        {
            AddHeight(removeModeProp);
            AddHeight(removeTargetProp);
            
            if (enabledTarget == EffectRemoveTarget.SpecificEffect || enabledTarget == EffectRemoveTarget.SpecificEffectFromSource)
                AddHeight(effectToRemoveProp);
            
            if (enabledRemoveMode == EffectRemoveMode.RemoveStacks)
                AddHeight(stacksToRemoveProp);
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

        // Get properties
        GetProperty("targetMode", out SerializedProperty targetModeProp);
        GetProperty("mode", out SerializedProperty modifyModeProp);
        GetProperty("effectToApply", out SerializedProperty effectToApplyProp);
        GetProperty("stacksToApply", out SerializedProperty stacksToApplyProp);
        GetProperty("removeMode", out SerializedProperty removeModeProp);
        GetProperty("removeTarget", out SerializedProperty removeTargetProp);
        GetProperty("effectToRemove", out SerializedProperty effectToRemoveProp);
        GetProperty("stacksToRemove", out SerializedProperty stacksToRemoveProp);


        DrawField(targetModeProp);
        DrawField(modifyModeProp);
        
        EffectModifyMode enabledMode = (EffectModifyMode)modifyModeProp.enumValueIndex;
        EffectRemoveMode enabledRemoveMode = (EffectRemoveMode)removeModeProp.enumValueIndex;
        EffectRemoveTarget enabledTarget = (EffectRemoveTarget)removeTargetProp.enumValueIndex;
       
        
        if (enabledMode == EffectModifyMode.Apply)
        {
            DrawField(effectToApplyProp);
            DrawField(stacksToApplyProp);
        }
        else
        {
            DrawField(removeModeProp);
            DrawField(removeTargetProp);
            
            if (enabledTarget == EffectRemoveTarget.SpecificEffect || enabledTarget == EffectRemoveTarget.SpecificEffectFromSource)
                DrawField(effectToRemoveProp);
            
            if (enabledRemoveMode == EffectRemoveMode.RemoveStacks)
                DrawField(stacksToRemoveProp);
        }        
    }
}