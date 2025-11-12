using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Hitbox Behavior", menuName = "Weapons/Behaviors/Hitbox")]
public class WeaponHitboxDefinition : WeaponBehaviorDefinition<WeaponHitbox, WeaponHitboxDefinition>
{
    public float activeDuration = 0.3f;
    protected override WeaponHitbox CreateTypedInstance(Weapon owner) => new(this, owner);
}