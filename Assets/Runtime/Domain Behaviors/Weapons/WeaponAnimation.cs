public class WeaponAnimation : WeaponBehavior<WeaponAnimationDefinition>
{
    public WeaponAnimation(WeaponAnimationDefinition def, Weapon owner) : base(def, owner) { }
    public override void OnAttack() =>
        Owner.Handler.wielder.GetComponent<AnimationHandler>().TriggerAbility(Definition.attackTrigger);

}
