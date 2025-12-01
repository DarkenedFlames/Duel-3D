using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Conditions/StatThresholdCondition")]
public class StatThresholdCondition : ActivationCondition
{
    public override bool IsMet(Ability ability)
    {
        CharacterResources resources = ability.Owner.CharacterResources;
        if (!resources.TryGetResource(ability.Definition.expendedResource, out CharacterResource resource)) return false;

        return resource.Value >= ability.Definition.resourceCost;
    }
}
