using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Homing Definition", menuName = "Projectiles/Behaviors/Movement")]
public class ProjectileMovementDefinition : ProjectileBehaviorDefinition<ProjectileMovement, ProjectileMovementDefinition>
{
    public ProjectileMovementType movementType = ProjectileMovementType.Straight;
    public float speed = 20f;
    public float turnSpeed = 90f;
    public float homingRange = 15f;
    protected override ProjectileMovement CreateTypedInstance(Projectile owner) => new(this, owner);
}
