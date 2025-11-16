using System;
using UnityEngine;

[Serializable]
public class GivesAbilityInstruction : IInstruction
{
    [Tooltip("Ability to give.")]
    public AbilityDefinition abilityDefinition;
    public void Execute(IInstructionContext context)
    {
        if (context.Actor.TryGetComponent(out AbilityHandler abilityHandler))
            abilityHandler.LearnAbility(abilityDefinition);
    }
}













