using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Stamina Cost Behavior", menuName = "Weapons/Behaviors/Stamina Cost")]
public class WeaponStaminaCostDefinition : WeaponBehaviorDefinition<WeaponStaminaCost, WeaponStaminaCostDefinition>
{
    [Header("Stamina Settings")]
    public float staminaCost = 10f;
    protected override WeaponStaminaCost CreateTypedInstance(Weapon owner) => new(this, owner);
}