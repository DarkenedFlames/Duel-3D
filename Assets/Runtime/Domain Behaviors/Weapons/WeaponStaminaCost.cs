public class WeaponStaminaCost : WeaponBehavior<WeaponStaminaCostDefinition>
{
    public WeaponStaminaCost(WeaponStaminaCostDefinition def, Weapon owner) : base(def, owner) { }

    public override void OnAttack() => Owner.Handler.wielder.GetComponent<StatsHandler>().SpendStamina(Definition.staminaCost);
    
    public override void OnUpdate(float deltaTime)
    {
        var stats = Owner.Handler.wielder.GetComponent<StatsHandler>();
        if (stats.stamina >= Definition.staminaCost) Owner.attackGate.Unlock(WeaponKey.Stamina);
        else Owner.attackGate.Lock(WeaponKey.Stamina);
    }
}
