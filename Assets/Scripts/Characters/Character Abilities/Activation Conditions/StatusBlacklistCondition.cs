using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Conditions/Status Blacklist")]
public class StatusBlacklistCondition : ActivationCondition
{
    [Tooltip("Statuses that the caster must not have in order to cast.")]
    public List<StatusDefinition> blacklist;

    public override bool IsMet(Ability ability)
    {
        if (!ability.GameObject.TryGetComponent(out CharacterStatuses characterStatuses))
        {
            Debug.LogError($"Activation Condition {nameof(StatusBlacklistCondition)} expected missing component: {nameof(CharacterStatuses)}!");
            return false;
        }

        foreach (StatusDefinition definition in blacklist)
            if (characterStatuses.TryGetStatus(definition, out CharacterStatus _)) 
                return false;
        
        return true;
    }
}
