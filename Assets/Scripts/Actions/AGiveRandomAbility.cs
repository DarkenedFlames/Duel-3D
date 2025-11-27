using UnityEditor.Rendering;
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
            Debug.LogError($"{nameof(AGiveAbility)} was passed a null parameter: {nameof(context.Target)}!");
            return;
        }
        if (set == null)
        {
            Debug.LogError($"{nameof(AGiveAbility)} was configured with a null parameter: {nameof(set)}!");
            return;
        }
        if (set.definitions.Count == 0)
        {
            Debug.LogError($"{nameof(AGiveAbility)} was configured with an invalid parameter: {nameof(set)} cannot be empty!");
            return;
        }
        if (!context.Target.TryGetComponent(out CharacterAbilities abilities))
        {
            Debug.LogError($"{nameof(AGiveAbility)} was passed a parameter with a missing component: {nameof(context.Target)} missing {nameof(CharacterAbilities)}!");
            return;
        }

        AbilityDefinition abilityToGive = set.GetAbilityWeightedByType();
        abilities.LearnAbility(abilityToGive);
    }
}