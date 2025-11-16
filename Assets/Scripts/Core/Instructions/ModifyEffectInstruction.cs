using System;
using UnityEngine;

[Serializable]
public class ModifyEffectInstruction : IInstruction
{
    [Tooltip("Effect to modify.")]
    public EffectDefinition effectDefinition;
    
    [Tooltip("Apply stacks if mode is checked, otherwise remove stacks.")]
    public bool mode;

    [Tooltip("The number of stacks to apply or remove.")]
    public int stacks;
    
    public void Execute(IInstructionContext context)
    {
        if (!context.Actor.TryGetComponent(out EffectHandler effects)) return;

        if (mode) 
            effects.ApplyEffect(effectDefinition, stacks);
        else
            effects.RemoveStacks(effectDefinition.effectName, stacks);
        
    }
}