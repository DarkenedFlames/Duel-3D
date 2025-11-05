using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Damage Behavior", menuName = "Weapons/Behaviors/Damage")]
public class WeaponDamageDefinition : WeaponBehaviorDefinition<WeaponDamage, WeaponDamageDefinition>
{
    [Header("Damage Settings")]
    public float damage = 10f;
    protected override WeaponDamage CreateTypedInstance(Weapon owner) => new(this, owner);
}