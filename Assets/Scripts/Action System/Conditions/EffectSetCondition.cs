using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class EffectSetCondition : IActionCondition
{
    public enum Mode
    {
        AllArePresent,
        NotAllArePresent,
        AnyArePresent,
        NoneArePresent
    }

    [Tooltip("The set of effects to check for."), SerializeField]
    EffectDefinitionSet EffectSet;
    [Tooltip("The target to check for effects."), SerializeField]
    ActionTargetMode targetMode = ActionTargetMode.Target;
    [Tooltip("The mode to use when checking effects."), SerializeField]
    Mode mode = Mode.AllArePresent;
    
    List<EffectDefinition> DefinitionList => EffectSet.Definitions;
    
    public bool IsSatisfied(ActionContext context)
    {
        Character target = targetMode switch
        {
            ActionTargetMode.Owner => context.Source.Owner,
            ActionTargetMode.Target => context.Target,
            _ => null
        };
        
        List<EffectDefinition> targetEffects = target.CharacterEffects.Effects.Select(e => e.Definition).ToList();
        
        bool all = DefinitionList.All(e => targetEffects.Contains(e));
        bool any = DefinitionList.Any(e => targetEffects.Contains(e));
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