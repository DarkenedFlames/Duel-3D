public class WeaponCooldown : WeaponBehavior<WeaponCooldownDefinition>
{
    public WeaponCooldown(WeaponCooldownDefinition def, Weapon owner) : base(def, owner) { }
    public override void OnAttack() => Owner.attackGate.Lock(WeaponKey.Cooldown, Definition.cooldown);
    
}
