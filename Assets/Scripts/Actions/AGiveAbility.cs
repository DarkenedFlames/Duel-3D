using UnityEngine;

public enum GiveAbilityMode { Specific, RandomBySlotFromSet }

[System.Serializable]
public class AGiveAbility : IGameAction
{
    [Header("Ability Configuration")]
    [Tooltip("Mode of Ability giving."), SerializeField]
    GiveAbilityMode mode = GiveAbilityMode.Specific;

    [Tooltip("Specific Ability to give."), SerializeField]
    AbilityDefinition abilityDefinition;

    [Tooltip("Set from which a random ability will be chosen and granted. 25% chance for each slot (Primary, Secondary, Utility, Special)."), SerializeField]
    AbilityDefinitionSet set;


    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Target), nameof(Execute), nameof(AGiveAbility), context.Source.GameObject);
            return;
        }

        CharacterAbilities abilities = context.Target.CharacterAbilities;
        switch (mode)
        {
            case GiveAbilityMode.Specific: 
                if (abilityDefinition != null) 
                    abilities.LearnAbility(abilityDefinition);
                break;

            case GiveAbilityMode.RandomBySlotFromSet:
                if (set != null && set.definitions.Count != 0)
                    abilities.LearnAbility(set.GetAbilityWeightedByType());
                break;
        }
    }
}