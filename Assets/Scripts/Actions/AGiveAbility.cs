using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum GiveAbilityMode { Specific, RandomBySlotFromSet }

[System.Serializable]
public class AGiveAbility : IGameAction
{
    [Header("Target Configuration")]
    [Tooltip("Who to give ability to: Owner (caster/summoner) or Target (hit character)."), SerializeField]
    ActionTargetMode targetMode = ActionTargetMode.Target;

    [Header("Ability Configuration")]
    [Tooltip("Mode of Ability giving."), SerializeField]
    GiveAbilityMode mode = GiveAbilityMode.Specific;

    [Tooltip("Specific Ability to give."), SerializeField]
    AbilityDefinition abilityDefinition;

    [Tooltip("Set from which a random ability will be chosen and granted. 25% chance for each slot (Primary, Secondary, Utility, Special)."), SerializeField]
    AbilityDefinitionSet set;


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

        CharacterAbilities abilities = target.CharacterAbilities;
        switch (mode)
        {
            case GiveAbilityMode.Specific: 
                if (abilityDefinition == null || abilities.HasAbility(abilityDefinition)) return;
                
                abilities.LearnAbility(abilityDefinition);
                break;

            case GiveAbilityMode.RandomBySlotFromSet:
                if (set == null || set.definitions.Count == 0) return;
                
                List<AbilityDefinition> ownedAbilities = abilities.abilities.Values
                    .Select(a => a.Definition)
                    .ToList();

                AbilityDefinition randomDefinition = set.GetAbilityWeightedByType(ownedAbilities);
                
                if (randomDefinition == null)
                {
                    Debug.LogWarning($"{target.gameObject.name} already has all abilities from the set!");
                    return;
                }

                abilities.LearnAbility(randomDefinition);
                break;
        }
    }
}