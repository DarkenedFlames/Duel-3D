using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Damage Definition", menuName = "Projectiles/Behaviors/Damage")]
public class ProjectileDamageDefinition : ProjectileBehaviorDefinition<ProjectileDamage, ProjectileDamageDefinition>
{
    public float impactDamage = 10.0f;
    protected override ProjectileDamage CreateTypedInstance(Projectile owner) => new(this, owner);
}