using UnityEngine;

public enum ModifyStatMode { AddModifier, RemoveModifier }
public enum ModifyStatTarget { Specific, All }
public enum StatModifierTarget { Specific, All, SpecificFromSource, AllFromSource }

[System.Serializable]
public class AModifyStat : IGameAction
{
    [Header("Target Configuration")]
    [Tooltip("Who to modify: Owner (caster/summoner) or Target (hit character)."), SerializeField]
    ActionTargetMode targetMode = ActionTargetMode.Target;

    [Header("Stat Configuration")]
    [Tooltip("Whether to add or remove the modifier."), SerializeField]
    ModifyStatMode mode = ModifyStatMode.AddModifier;

    [Tooltip("Targeting mode for which stats to modify."), SerializeField]
    ModifyStatTarget targetStat = ModifyStatTarget.Specific;

    [Tooltip("The stat to modify."), SerializeField]
    StatType statType;

    [Tooltip("Targeting mode for which modifiers to modify."), SerializeField]
    StatModifierTarget targetModifier = StatModifierTarget.Specific;

    [Tooltip("The type of modifier to modify."), SerializeField]
    StatModifierType targetModifierType = StatModifierType.Flat;

    [Tooltip("The modifier value to modify."), SerializeField]
    float amount = 0f;

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
            Debug.LogWarning($"{nameof(AModifyStat)}: {targetMode} is null. Action skipped.");
            return;
        }
        
        CharacterStats stats = target.CharacterStats;
        switch (mode)
        {
            case ModifyStatMode.AddModifier: stats.AddModifierToStat(statType, targetModifierType, amount, context.Source); break;
            case ModifyStatMode.RemoveModifier:
                switch (targetStat)
                {
                    case ModifyStatTarget.Specific:
                        switch (targetModifier)
                        {
                            case StatModifierTarget.Specific:           stats.RemoveSpecificModifierFromStat(statType, targetModifierType, amount, source: null); break;
                            case StatModifierTarget.SpecificFromSource: stats.RemoveSpecificModifierFromStat(statType, targetModifierType, amount, context.Source); break;
                            case StatModifierTarget.All:                stats.RemoveAllModifiersFromStat(statType, source: null); break;
                            case StatModifierTarget.AllFromSource:      stats.RemoveAllModifiersFromStat(statType, context.Source); break;
                        }
                        break;
                    case ModifyStatTarget.All:
                        switch (targetModifier)
                        {
                            case StatModifierTarget.Specific:           stats.RemoveSpecificModifierFromAllStats(targetModifierType, amount, source: null); break;
                            case StatModifierTarget.SpecificFromSource: stats.RemoveSpecificModifierFromAllStats(targetModifierType, amount, context.Source); break;
                            case StatModifierTarget.All:                stats.RemoveAllModifiersFromAllStats(source: null); break;
                            case StatModifierTarget.AllFromSource:      stats.RemoveAllModifiersFromAllStats(context.Source); break;
                        }
                        break;
                }
                break;
        }
    }
}
