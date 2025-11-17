using UnityEngine;

[System.Serializable]
public class LModifyEffect : EventReaction
{
    [Header("ModifyEffect Configuration")]
    [Tooltip("Effect to modify."), SerializeField]
    EffectDefinition effectDefinition;
    
    [Tooltip("Apply stacks if mode is checked, otherwise remove stacks."), SerializeField]
    bool mode;

    [Tooltip("The number of stacks to apply or remove."), SerializeField]
    int stacks;
    
    public override void OnEvent(EventContext context)
    {
        if (context is PositionContext cxt)
            if (cxt.target.TryGetComponent(out EffectHandler effects))
                if (mode) 
                    effects.ApplyEffect(effectDefinition, stacks);
                else
                    effects.RemoveStacks(effectDefinition.effectName, stacks);        
    }
}