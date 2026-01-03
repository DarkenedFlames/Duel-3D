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

    [Tooltip("The number + icon prefab to use (NumberIconUI)"), SerializeField]
    GameObject numberIconUI;

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

        Dictionary<StatDefinition, List<StatModifier>> added = new();
        Dictionary<StatDefinition, List<StatModifier>> removed = new();
        
        CharacterStats stats = target.CharacterStats;
        switch (mode)
        {
            case Mode.AddModifierToSpecificStat:
                added = stats.AddModifiers(targetModifierType, amount, statDefinition.statType, context.Source);
                break;

            case Mode.AddModifierToRandomStatFromSet:
                if (statDefinitions.TryGetRandomStatDefinition(out StatDefinition randomDefinition))
                    added = stats.AddModifiers(targetModifierType, amount, randomDefinition.statType, context.Source);
                break;

            case Mode.AddModifierToAllStatsFromSet:
                foreach (StatDefinition definition in statDefinitions.Definitions)
                {
                    added[definition] = stats.AddModifiers(
                        targetModifierType, 
                        amount,
                        definition.statType,
                        context.Source
                    )[definition];
                }
                break;

            case Mode.AddModifierToAllStats:
                added = stats.AddModifiers(targetModifierType, amount, null, context.Source);
                break;

            case Mode.RemoveSpecificModifierFromSpecificStat:
                removed = stats.RemoveModifiers(statDefinition.statType, targetModifierType, amount, removeOnlyFromSource ? context.Source : null);
                break;

            case Mode.RemoveSpecificModifierFromAllStats:
                removed = stats.RemoveModifiers(null, targetModifierType, amount, removeOnlyFromSource ? context.Source : null);
                break;

            case Mode.RemoveAllModifiersFromSpecificStat:
                removed = stats.RemoveModifiers(statDefinition.statType, null, null, removeOnlyFromSource ? context.Source : null);
                break;

            case Mode.RemoveAllModifiersFromAllStats:
                removed = stats.RemoveModifiers(null, null, null, removeOnlyFromSource ? context.Source : null);
                break;
        }
        
        foreach (KeyValuePair<StatDefinition, List<StatModifier>> kvp in added)
            foreach (StatModifier modifier in kvp.Value)
                SpawnModifierUI(kvp.Key, modifier, target.transform);
        
        /* For now, this just clutters the UI too much
        foreach (KeyValuePair<StatDefinition, List<StatModifier>> kvp in removed)
            foreach (StatModifier modifier in kvp.Value)
                SpawnModifierUI(kvp.Key, modifier, Color.orange, target.transform);
        */
    }    
    
    void SpawnModifierUI(StatDefinition definition, StatModifier modifier, Transform targetTransform)
    {
        if (numberIconUI == null)
            return;
    
        GameObject spawnedUI = Object.Instantiate(numberIconUI, targetTransform.position, targetTransform.rotation);
        if (!spawnedUI.TryGetComponent(out NumberIconUI uiComponent))
        {
            Debug.LogError($"{spawnedUI.name} is missing {nameof(NumberIconUI)}");
            return;
        }

        string text = modifier.Type switch
        {
            StatModifierType.Flat        => modifier.Value >= 0 ? "↑" : "↓",
            StatModifierType.PercentAdd  => modifier.Value >= 0 ? "↑" : "↓",
            StatModifierType.PercentMult => modifier.Value >= 1 ? "↑" : "↓",
            _ => "",
        };

        uiComponent.Initialize(definition.Icon, text, Color.black);
    }
}
