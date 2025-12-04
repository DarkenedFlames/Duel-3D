using UnityEngine;

public enum ModifyStatMode { AddModifier, RemoveModifier }
public enum ModifyStatTarget { Specific, All }
public enum StatModifierTarget { Specific, All, SpecificFromSource, AllFromSource }

[System.Serializable]
public class AModifyStat : IGameAction
{
    [Header("Stat Configuration")]
    [Tooltip("Whether to add or remove the modifier."), SerializeField]
    ModifyStatMode mode = ModifyStatMode.AddModifier;

    [Tooltip("Targeting mode for which stats to modify."), SerializeField]
    ModifyStatTarget target = ModifyStatTarget.Specific;

    [Tooltip("The stat to modify."), SerializeField]
    StatDefinition StatDefinition;

    [Tooltip("Targeting mode for which modifiers to modify."), SerializeField]
    StatModifierTarget modifierTarget = StatModifierTarget.Specific;

    [Tooltip("The type of modifier to modify."), SerializeField]
    StatModifierType type = StatModifierType.Flat;

    [Tooltip("The modifier value to modify."), SerializeField]
    float amount = 0f;

    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Target), nameof(Execute), nameof(AModifyStat), context.Source.GameObject);
            return;
        }
        if (context.Source == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Source), nameof(Execute), nameof(AModifyStat), context.Source.GameObject);
            return;
        }
        
        CharacterStats stats = context.Target.CharacterStats;
        switch (mode)
        {
            case ModifyStatMode.AddModifier: stats.AddModifierToStat(StatDefinition, type, amount, context.Source); break;
            case ModifyStatMode.RemoveModifier:
                switch (target)
                {
                    case ModifyStatTarget.Specific:
                        switch (modifierTarget)
                        {
                            case StatModifierTarget.Specific:           stats.RemoveSpecificModifierFromStat(StatDefinition, type, amount, source: null); break;
                            case StatModifierTarget.SpecificFromSource: stats.RemoveSpecificModifierFromStat(StatDefinition, type, amount, context.Source); break;
                            case StatModifierTarget.All:                stats.RemoveAllModifiersFromStat(StatDefinition, source: null); break;
                            case StatModifierTarget.AllFromSource:      stats.RemoveAllModifiersFromStat(StatDefinition, context.Source); break;
                        }
                        break;
                    case ModifyStatTarget.All:
                        switch (modifierTarget)
                        {
                            case StatModifierTarget.Specific:           stats.RemoveSpecificModifierFromAllStats(type, amount, source: null); break;
                            case StatModifierTarget.SpecificFromSource: stats.RemoveSpecificModifierFromAllStats(type, amount, context.Source); break;
                            case StatModifierTarget.All:                stats.RemoveAllModifiersFromAllStats(source: null); break;
                            case StatModifierTarget.AllFromSource:      stats.RemoveAllModifiersFromAllStats(context.Source); break;
                        }
                        break;
                }
                break;
        }
    }
}
