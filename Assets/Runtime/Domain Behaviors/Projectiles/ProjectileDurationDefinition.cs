using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Duration Definition", menuName = "Projectiles/Behaviors/Duration")]
public class ProjectileDurationDefinition : ProjectileBehaviorDefinition<ProjectileDuration, ProjectileDurationDefinition>
{
    public float duration = 5.0f;
    protected override ProjectileDuration CreateTypedInstance(Projectile owner) => new(this, owner);
}