using UnityEngine;

[CreateAssetMenu(fileName = "New Ability Animation Definition", menuName = "Abilities/Behaviors/Animation")]
public class AbilityAnimationDefinition : AbilityBehaviorDefinition<AbilityAnimation, AbilityAnimationDefinition>
{
    public string castTrigger = "AbilityTrigger";
    protected override AbilityAnimation CreateTypedInstance(Ability owner) => new(this, owner);
}