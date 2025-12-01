using UnityEngine;

[System.Serializable]
public class AModifyEffect : IGameAction
{
    [Header("Effect Configuration")]
    [Tooltip("Effect to modify."), SerializeField]
    EffectDefinition effectDefinition;
    
    [Tooltip("Apply stacks if mode is checked, otherwise remove stacks."), SerializeField]
    bool mode = true;

    [Tooltip("The number of stacks to apply or remove."), SerializeField, Min(1)]
    int stacks = 1;
    
    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Target), nameof(Execute), nameof(AModifyEffect), context.Source.GameObject);
            return;
        }
        if (effectDefinition == null)
        {
            LogFormatter.LogNullField(nameof(AModifyEffect), nameof(effectDefinition), context.Source.GameObject);
            return;
        }

        if (mode) 
            context.Target.CharacterEffects.AddEffect(effectDefinition, stacks);
        else
            context.Target.CharacterEffects.RemoveEffect(effectDefinition, stacks);
    }
}