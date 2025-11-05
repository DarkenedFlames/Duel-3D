using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Knockback Definition", menuName = "Projectiles/Behaviors/Knockback")]
public class ProjectileKnockbackDefiniton : ProjectileBehaviorDefinition<ProjectileKnockback, ProjectileKnockbackDefiniton>
{
    public float force = 10.0f;
    protected override ProjectileKnockback CreateTypedInstance(Projectile owner) => new(this, owner);
}