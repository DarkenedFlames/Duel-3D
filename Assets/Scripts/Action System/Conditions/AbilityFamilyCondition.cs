using UnityEngine;
using System.Linq;

[System.Serializable]
public class AbilityFamilyCondition : IActionCondition
{
    public enum Mode
    {
        AtLeast,
        AtMost,
        Exactly
    }

    [SerializeField] ActionTargetMode targetMode = ActionTargetMode.Owner;
    [SerializeField] AbilityFamily family = AbilityFamily.Arcana;
    [SerializeField] Mode mode = Mode.AtLeast;
    [SerializeField] int count = 1;
       
    public bool IsSatisfied(ActionContext context)
    {
        Character target = targetMode switch
        {
            ActionTargetMode.Owner => context.Source.Owner,
            ActionTargetMode.Target => context.Target,
            _ => null
        };
        
        int matches = target.CharacterAbilities.abilities.Values
            .ToList()
            .Count(a => a.Definition.Family == family);

        bool satisfied = mode switch
        {
            Mode.AtLeast => matches >= count,
            Mode.AtMost  => matches <= count,
            Mode.Exactly => matches == count,
            _ => false
        };
        
        return satisfied;
    }
}