public class WeaponHitbox : WeaponBehavior<WeaponHitboxDefinition>
{
    public WeaponHitbox(WeaponHitboxDefinition def, Weapon owner) : base(def, owner) { }

    public override void OnSpawn() => Owner.triggerGate.Lock(WeaponKey.InactiveHitbox);

    public override void OnAttack() => Owner.triggerGate.Unlock(WeaponKey.InactiveHitbox, Definition.activeDuration);
}
