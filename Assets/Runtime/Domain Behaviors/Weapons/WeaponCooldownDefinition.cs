using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Cooldown Behavior", menuName = "Weapons/Behaviors/Cooldown")]
public class WeaponCooldownDefinition : WeaponBehaviorDefinition<WeaponCooldown, WeaponCooldownDefinition>
{
    [Header("Cooldown Settings")]
    public float cooldown = 1f;
    protected override WeaponCooldown CreateTypedInstance(Weapon owner) => new(this, owner);
}