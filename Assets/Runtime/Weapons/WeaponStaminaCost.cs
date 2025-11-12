using Unity.VisualScripting;

public class WeaponStaminaCost : WeaponBehavior<WeaponStaminaCostDefinition>
{
    public WeaponStaminaCost(WeaponStaminaCostDefinition def, Weapon owner) : base(def, owner) { }

    public override void OnAttack()
    {
        StatsHandler stats = Owner.Handler.wielder.GetComponent<StatsHandler>();
        stats.TryModifyStat(StatType.Stamina, modifyMax: false, -1f * Definition.staminaCost);
    }
    
    public override void OnUpdate(float deltaTime)
    {
        StatsHandler stats = Owner.Handler.wielder.GetComponent<StatsHandler>();
        if (stats.GetStat(StatType.Stamina, getMax: false) >= Definition.staminaCost) Owner.attackGate.Unlock(WeaponKey.Stamina);
        else Owner.attackGate.Lock(WeaponKey.Stamina);
    }
}
