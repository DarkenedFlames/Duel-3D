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
            Debug.LogError($"{nameof(AGiveAbility)} was passed a null parameter: {nameof(context.Target)}!");
            return;
        }
        if (abilityDefinition == null)
        {
            Debug.LogError($"{nameof(AGiveAbility)} was configured with a null parameter: {nameof(abilityDefinition)}!");
            return;
        }
        if (!context.Target.TryGetComponent(out CharacterAbilities abilities))
        {
            Debug.LogError($"{nameof(AGiveAbility)} was passed a parameter with a missing component: {nameof(context.Target)} missing {nameof(CharacterAbilities)}!");
            return;
        }

        abilities.LearnAbility(abilityDefinition);
    }
}