using UnityEngine;

[System.Serializable]
public class EffectStacksCondition : IActionCondition
{
    public enum Mode
    {
        AtLeast,
        AtMost,
        Exactly
    }

    [Tooltip("The target to check for effect stacks."), SerializeField]
    ActionTargetMode targetMode = ActionTargetMode.Target;

    [Tooltip("The effect definition to check for."), SerializeField]
    EffectDefinition effectDefinition;

    [Tooltip("The mode to use when comparing stacks."), SerializeField]
    Mode mode = Mode.AtLeast;

    [Tooltip("The number of stacks to compare against."), SerializeField]
    int stackCount = 1;


    public bool IsSatisfied(ActionContext context)
    {
        Character target = targetMode switch
        {
            ActionTargetMode.Owner => context.Source.Owner,
            ActionTargetMode.Target => context.Target,
            _ => null
        };

        int currentStacks = target.CharacterEffects.TryGetEffect(effectDefinition, out CharacterEffect effect)
            ? effect.currentStacks.Value
            : 0;

        bool satisfied = mode switch
        {
            Mode.AtLeast => currentStacks >= stackCount,
            Mode.AtMost  => currentStacks <= stackCount,
            Mode.Exactly => currentStacks == stackCount,
            _ => false
        };
        
        return satisfied;
    }
}