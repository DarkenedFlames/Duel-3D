using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AModifyStat : IGameAction
{
    public enum Mode { 
        AddModifierToSpecificStat,
        AddModifierToRandomStatFromSet,
        AddModifierToAllStatsFromSet,
        AddModifierToAllStats,
        RemoveSpecificModifierFromSpecificStat,
        RemoveSpecificModifierFromAllStats,
        RemoveAllModifiersFromSpecificStat,
        RemoveAllModifiersFromAllStats,
    }

    [Header("Conditions")]
    [SerializeReference]
    public List<IActionCondition> Conditions;

    [Header("Action Configuration")]
    [Tooltip("Who to modify: Owner (caster/summoner) or Target (hit character)."), SerializeField]
    ActionTargetMode targetMode = ActionTargetMode.Target;

    [Tooltip("Whether to add or remove the modifier."), SerializeField]
    Mode mode = Mode.AddModifierToSpecificStat;

    [Tooltip("Whether to only remove modifiers that were added by this."), SerializeField]
    bool removeOnlyFromSource = true;


    [Tooltip("The stat to modify."), SerializeField]
    StatDefinition statDefinition;

    [Tooltip("The set of stat definitions to choose from."), SerializeField]
    StatDefinitionSet statDefinitions;

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

        if (Conditions != null)
        {
            foreach (IActionCondition condition in Conditions)
                if (!condition.IsSatisfied(context))
                    return;
        }
        
        CharacterStats stats = target.CharacterStats;
        switch (mode)
        {
            case Mode.AddModifierToSpecificStat:
                stats.AddModifiers(targetModifierType, amount, statDefinition.statType, context.Source);
                break;

            case Mode.AddModifierToRandomStatFromSet:
                if (statDefinitions.TryGetRandomStatDefinition(out StatDefinition randomDefinition))
                    stats.AddModifiers(targetModifierType, amount, randomDefinition.statType, context.Source);
                break;

            case Mode.AddModifierToAllStatsFromSet:
                foreach (StatDefinition definition in statDefinitions.Definitions)
                    stats.AddModifiers(targetModifierType, amount, definition.statType, context.Source);
                break;

            case Mode.AddModifierToAllStats:
                stats.AddModifiers(targetModifierType, amount, null, context.Source);
                break;

            case Mode.RemoveSpecificModifierFromSpecificStat:
                stats.RemoveModifiers(statDefinition.statType, targetModifierType, amount, removeOnlyFromSource ? context.Source : null);
                break;

            case Mode.RemoveSpecificModifierFromAllStats:
                stats.RemoveModifiers(null, targetModifierType, amount, removeOnlyFromSource ? context.Source : null);
                break;

            case Mode.RemoveAllModifiersFromSpecificStat:
                stats.RemoveModifiers(statDefinition.statType, null, null, removeOnlyFromSource ? context.Source : null);
                break;

            case Mode.RemoveAllModifiersFromAllStats:
                stats.RemoveModifiers(null, null, null, removeOnlyFromSource ? context.Source : null);
                break;
        }
    }
}
