using UnityEngine;

public class WeaponDamage : WeaponBehavior<WeaponDamageDefinition>
{
    public WeaponDamage(WeaponDamageDefinition def, Weapon owner) : base(def, owner) { }
    public override void OnTrigger(Collider other)
    {
        if (other.gameObject.TryGetComponent(out StatsHandler targetStats))
        {
            targetStats.TakeDamage(Definition.damage);
            Debug.Log($"{Owner.Handler.wielder.name}'s weapon hit {other.gameObject.name} for {Definition.damage} damage!");
        }
    }
}
