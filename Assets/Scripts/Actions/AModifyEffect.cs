using UnityEngine;

public enum EffectModifyMode { Apply, Remove }
public enum EffectRemoveMode { RemoveStacks, RemoveAll }
public enum EffectRemoveTarget { SpecificEffect, SpecificEffectFromSource, AllEffects, AllFromSource }


[System.Serializable]
public class AModifyEffect : IGameAction
{
    [Header("Target Configuration")]
    [SerializeField, Tooltip("Who to modify: Owner (caster/summoner) or Target (hit character).")] 
    private ActionTargetMode targetMode = ActionTargetMode.Target;

    [SerializeField] private EffectModifyMode mode;

    [Header("Apply Settings")]
    [SerializeField] private EffectDefinition effectToApply;
    [SerializeField, Min(1)] private int stacksToApply = 1;

    [Header("Remove Settings")]
    [SerializeField] private EffectRemoveMode removeMode;
    [SerializeField] private EffectRemoveTarget removeTarget;

    [SerializeField] private EffectDefinition effectToRemove;
    [SerializeField, Min(1)] private int stacksToRemove = 1;
    
    public void Execute(ActionContext context)
    {
        Character target = targetMode switch
        {
            ActionTargetMode.Owner => context.Source.Owner,
            ActionTargetMode.Target => context.Target,
            _ => null,
        };

        if (target == null)
        {
            Debug.LogWarning($"{nameof(AModifyEffect)}: {targetMode} is null. Action skipped.");
            return;
        }

        CharacterEffects effects = target.CharacterEffects;

        if (mode == EffectModifyMode.Apply)
        {
            if (effectToApply == null)
            {
                LogFormatter.LogNullField(nameof(effectToApply), nameof(AModifyEffect), context.Source.GameObject);
                return;
            }

            effects.AddEffect(effectToApply, stacksToApply, context.Source);
            return;
        }

        // REMOVE LOGIC
        switch (removeTarget)
        {
            case EffectRemoveTarget.SpecificEffect:
                if (effectToRemove == null) return;
                if (removeMode == EffectRemoveMode.RemoveStacks)
                    effects.RemoveEffect(effectToRemove, stacksToRemove);
                else
                    effects.RemoveEffect(effectToRemove);
                break;

            case EffectRemoveTarget.SpecificEffectFromSource:
                if (effectToRemove == null) return;
                if (removeMode == EffectRemoveMode.RemoveStacks)
                    effects.RemoveSpecificEffectFromSource(effectToRemove, stacksToRemove, context.Source);
                else
                    effects.RemoveSpecificEffectFromSource(effectToRemove, context.Source);
                break;

            case EffectRemoveTarget.AllEffects:
                effects.RemoveAllEffects();
                break;

            case EffectRemoveTarget.AllFromSource:
                effects.RemoveAllEffectsFromSource(context.Source);
                break;
        }
    }
}
