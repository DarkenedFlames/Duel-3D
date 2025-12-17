using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum GiveAbilityMode { Specific, RandomBySlotFromSet, RandomByFamilyFromSet }

[System.Serializable]
public class AGiveAbility : IGameAction
{
    [Header("Conditions")]
    [SerializeReference]
    public List<IActionCondition> Conditions;

    [Header("Action Configuration")]
    [Tooltip("Who to give ability to: Owner (caster/summoner) or Target (hit character)."), SerializeField]
    ActionTargetMode targetMode = ActionTargetMode.Target;

    [Tooltip("Mode of Ability giving."), SerializeField]
    GiveAbilityMode mode = GiveAbilityMode.Specific;

    [Tooltip("Specific Ability to give."), SerializeField]
    AbilityDefinition abilityDefinition;

    [Tooltip("Set from which a random ability will be chosen and granted. 25% chance for each slot (Primary, Secondary, Utility, Special)."), SerializeField]
    AbilityDefinitionSet set;

    [Tooltip("With Random By Family From Set, the chosen family to choose from."), SerializeField]
    AbilityFamily family;


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
            Debug.LogWarning($"{nameof(AGiveAbility)}: {targetMode} is null. Action skipped.");
            return;
        }

        if (Conditions != null)
        {
            foreach (IActionCondition condition in Conditions)
                if (!condition.IsSatisfied(context))
                    return;
        }

        if (set == null || set.definitions.Count == 0)
        {
            LogFormatter.LogNullCollectionField(nameof(set), nameof(Execute), nameof(AGiveAbility), context.Source.GameObject);
            return;
        }

        CharacterAbilities abilities = target.CharacterAbilities;

        List<AbilityDefinition> ownedAbilities = abilities.abilities.Values
            .Select(a => a.Definition)
            .ToList();

        AbilityDefinition definitionToLearn = mode switch
        {
            GiveAbilityMode.Specific => ownedAbilities.Contains(abilityDefinition) ? null : abilityDefinition,
            GiveAbilityMode.RandomBySlotFromSet => set.GetAbilityWeightedByType(ownedAbilities),
            GiveAbilityMode.RandomByFamilyFromSet => set.GetAbilityOfFamily(family),
            _ => null,
        };

        if (definitionToLearn != null)
            abilities.LearnAbility(definitionToLearn);
    }
}