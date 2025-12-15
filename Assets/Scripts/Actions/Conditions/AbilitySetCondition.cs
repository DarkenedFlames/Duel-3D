using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class AbilitySetCondition : IActionCondition
{
    public enum Mode
    {
        AllArePresent,
        NotAllArePresent,
        AnyArePresent,
        NoneArePresent
    }

    [Header("Condition Settings")]
    [Tooltip("The set of abilities to check for."), SerializeField]
    AbilityDefinitionSet abilitySet;
    [Tooltip("The target to check for abilities."), SerializeField]
    ActionTargetMode targetMode = ActionTargetMode.Target;
    [Tooltip("The mode to use when checking abilities."), SerializeField]
    Mode mode = Mode.AllArePresent;
    
    List<AbilityDefinition> DefinitionList => abilitySet.definitions;
    
    public bool IsSatisfied(ActionContext context)
    {
        Character target = targetMode switch
        {
            ActionTargetMode.Owner => context.Source.Owner,
            ActionTargetMode.Target => context.Target,
            _ => null
        };
        
        List<AbilityDefinition> targetAbilities = target.CharacterAbilities.abilities.Values.Select(a => a.Definition).ToList();
        
        bool all = DefinitionList.All(a => targetAbilities.Contains(a));
        bool any = DefinitionList.Any(a => targetAbilities.Contains(a));
        bool satisfied = mode switch
        {
            Mode.AllArePresent => all,
            Mode.AnyArePresent => any,
            Mode.NotAllArePresent => !all,
            Mode.NoneArePresent => !any,
            _ => false
        };
        return satisfied;
    }
}