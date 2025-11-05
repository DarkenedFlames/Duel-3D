public class AbilityAnimation : AbilityBehavior<AbilityAnimationDefinition>
{
    public AbilityAnimation(AbilityAnimationDefinition def, Ability owner) : base(def, owner) { }
    public override void OnUpdate(float deltaTime) { }
    public override void OnCast() =>
        Owner.Handler.gameObject.GetComponent<AnimationHandler>().TriggerAbility(Definition.castTrigger);

}
