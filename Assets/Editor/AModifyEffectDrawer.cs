using UnityEditor;
using UnityEngine;

[ActionDrawer(typeof(AModifyEffect))]
public class AModifyEffectDrawer : IActionTypeDrawer
{
    const float VSpace = 2f;

    public float GetHeight(SerializedProperty property, GUIContent label)
    {
        float height = 0f;

        var modifyModeProp = property.FindPropertyRelative("mode");
        var effectToApplyProp = property.FindPropertyRelative("effectToApply");
        var stacksToApplyProp = property.FindPropertyRelative("stacksToApply");
        var removeModeProp = property.FindPropertyRelative("removeMode");
        var removeTargetProp = property.FindPropertyRelative("removeTarget");
        var effectToRemoveProp = property.FindPropertyRelative("effectToRemove");
        var stacksToRemoveProp = property.FindPropertyRelative("stacksToRemove");

        EffectModifyMode enabledMode = (EffectModifyMode)modifyModeProp.enumValueIndex;
        EffectRemoveMode enabledRemoveMode = (EffectRemoveMode)removeModeProp.enumValueIndex;
        EffectRemoveTarget enabledTarget = (EffectRemoveTarget)removeTargetProp.enumValueIndex;

        height += EditorGUI.GetPropertyHeight(modifyModeProp, true) + VSpace;
        
        if (enabledMode == EffectModifyMode.Apply)
        {
            height += EditorGUI.GetPropertyHeight(effectToApplyProp) + VSpace;
            height += EditorGUI.GetPropertyHeight(stacksToApplyProp) + VSpace;
        }
        else
        {
            height += EditorGUI.GetPropertyHeight(removeModeProp) + VSpace;
            height += EditorGUI.GetPropertyHeight(removeTargetProp) + VSpace;
            
            if (enabledTarget == EffectRemoveTarget.SpecificEffect || enabledTarget == EffectRemoveTarget.SpecificEffectFromSource)
                height += EditorGUI.GetPropertyHeight(effectToRemoveProp) + VSpace;
            
            if (enabledRemoveMode == EffectRemoveMode.RemoveStacks)
                height += EditorGUI.GetPropertyHeight(stacksToRemoveProp) + VSpace;
        }

        return height;
    }

    public void Draw(SerializedProperty property, Rect position, GUIContent label)
    {
        float y = position.y;

        // Get properties
        var modifyModeProp = property.FindPropertyRelative("mode");
        var effectToApplyProp = property.FindPropertyRelative("effectToApply");
        var stacksToApplyProp = property.FindPropertyRelative("stacksToApply");
        var removeModeProp = property.FindPropertyRelative("removeMode");
        var removeTargetProp = property.FindPropertyRelative("removeTarget");
        var effectToRemoveProp = property.FindPropertyRelative("effectToRemove");
        var stacksToRemoveProp = property.FindPropertyRelative("stacksToRemove");

        float h;

        h = EditorGUI.GetPropertyHeight(modifyModeProp);
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), modifyModeProp);
        y += h + 2;
        
        EffectModifyMode enabledMode = (EffectModifyMode)modifyModeProp.enumValueIndex;
        EffectRemoveMode enabledRemoveMode = (EffectRemoveMode)removeModeProp.enumValueIndex;
        EffectRemoveTarget enabledTarget = (EffectRemoveTarget)removeTargetProp.enumValueIndex;
       
        
        if (enabledMode == EffectModifyMode.Apply)
        {
            h = EditorGUI.GetPropertyHeight(effectToApplyProp);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), effectToApplyProp);
            y += h + 2;
            
            h = EditorGUI.GetPropertyHeight(stacksToApplyProp);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), stacksToApplyProp);
            y += h + 2;
        }
        else
        {
            h = EditorGUI.GetPropertyHeight(removeModeProp);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), removeModeProp);
            y += h + 2;
        
            h = EditorGUI.GetPropertyHeight(removeTargetProp);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), removeTargetProp);
            y += h + 2;
            
            if (enabledTarget == EffectRemoveTarget.SpecificEffect || enabledTarget == EffectRemoveTarget.SpecificEffectFromSource)
            {
                h = EditorGUI.GetPropertyHeight(effectToRemoveProp);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), effectToRemoveProp);
                y += h + 2;
            }
            
            if (enabledRemoveMode == EffectRemoveMode.RemoveStacks)
            {
                h = EditorGUI.GetPropertyHeight(stacksToRemoveProp);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), stacksToRemoveProp);
                y += h + 2;
            }
        }        
    }
}