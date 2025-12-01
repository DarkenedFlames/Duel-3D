using UnityEngine;

[System.Serializable]
public class AGiveAbility : IGameAction
{
    [Header("Ability Configuration")]
    [Tooltip("Ability to give."), SerializeField]
    AbilityDefinition abilityDefinition;

    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Target), nameof(Execute), nameof(AGiveAbility), context.Source.GameObject);
            return;
        }
        if (abilityDefinition == null)
        {
            LogFormatter.LogNullField(nameof(AGiveAbility), nameof(abilityDefinition), context.Source.GameObject);
            return;
        }

        context.Target.CharacterAbilities.LearnAbility(abilityDefinition);
    }
}