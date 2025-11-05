using UnityEngine;

[CreateAssetMenu(fileName = "New Fires Projectile Behavior", menuName = "Abilities/Behaviors/Fires Projectile")]
public class FiresProjectileDefinition : AbilityBehaviorDefinition<FiresProjectile, FiresProjectileDefinition>
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public int projectileCount = 1;
    public float spreadAngle = 0f;
    public Vector3 originOffset = Vector3.forward;
    public Vector3 directionOffset = Vector3.zero;

    protected override FiresProjectile CreateTypedInstance(Ability owner) => new(this, owner);
}