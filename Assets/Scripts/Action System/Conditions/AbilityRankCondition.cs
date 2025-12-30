using UnityEngine;


[System.Serializable]
public class AbilityRankCondition : IActionCondition
{
    public enum Mode
    {
        AtLeast,
        AtMost,
        Exactly
    }

    [SerializeField] ActionTargetMode targetMode = ActionTargetMode.Owner;
    [SerializeField] AbilityDefinition abilityToCompare;
    [SerializeField] Mode mode = Mode.AtLeast;
    [SerializeField, Range(1,5)] int compareRank = 1;
    
    public bool IsSatisfied(ActionContext context)
    {
        Character target = targetMode switch
        {
            ActionTargetMode.Owner => context.Source.Owner,
            ActionTargetMode.Target => context.Target,
            _ => null
        };

        int currentRank = target.CharacterAbilities.TryGetAbility(abilityToCompare, out Ability ability)
            ? ability.Rank
            : 0;
        
        bool satisfied = mode switch
        {
            Mode.AtLeast => currentRank >= compareRank,
            Mode.AtMost  => currentRank <= compareRank,
            Mode.Exactly => currentRank == compareRank,
            _ => false
        };

        return satisfied;
    }
}