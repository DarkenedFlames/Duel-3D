using UnityEngine;
// Change this to become a condition that checks if a certain stat on the caster meets a certain threshold
// Rather than just the expended stat
[CreateAssetMenu(menuName = "Abilities/Conditions/StatThresholdCondition")]
public class StatThresholdCondition : ActivationCondition
{
    public override bool IsMet(Ability ability)
    {
        CharacterResource resource = ability.Owner.CharacterResources.GetResource(ability.Definition.ExpendedResource, this);
        return resource.Value >= ability.Definition.resourceCost;
    }
}
