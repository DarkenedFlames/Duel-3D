using UnityEngine;

[System.Serializable]
public class AGiveRandomAbility : IGameAction
{
    [Header("Ability Configuration")]
    [Tooltip("List of Abilities to select from."), SerializeField]
    AbilityDefinitionSet set;

    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Target), nameof(Execute), nameof(AGiveRandomAbility), context.Source.GameObject);
            return;
        }
        if (set == null || set.definitions.Count == 0)
        {
            LogFormatter.LogNullCollectionField(nameof(set), nameof(Execute), nameof(AGiveRandomAbility), context.Source.GameObject);
            return;
        }

        AbilityDefinition abilityToGive = set.GetAbilityWeightedByType();
        context.Target.CharacterAbilities.LearnAbility(abilityToGive);
    }
}