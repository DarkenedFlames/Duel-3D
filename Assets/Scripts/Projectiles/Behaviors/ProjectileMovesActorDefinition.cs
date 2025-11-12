using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Moves Actor Behavior", menuName = "Duel/Projectiles/Behaviors/MovesActor")]
public class ProjectileMovesActorDefinition : ProjectileBehaviorDefinition
{

    [Header("Movement Configs")]
    public MoveActorConfig[] sourceConfigs;
    public MoveActorConfig[] targetConfigs;
    public MoveActorConfig[] otherConfigs;

    public override ProjectileBehavior CreateRuntimeBehavior(Projectile projectile) => new ProjectileMovesActor(projectile, this);
}

public class ProjectileMovesActor : ProjectileBehavior
{
    private new ProjectileMovesActorDefinition Definition => (ProjectileMovesActorDefinition)base.Definition;
    public ProjectileMovesActor(Projectile projectile, ProjectileMovesActorDefinition definition) : base(projectile, definition) { }

    private void MoveActor(GameObject actor, HookType type)
    {
        MoveActorConfig[] configs = actor switch
        {
            _ when actor == Projectile.SourceActor => Definition.sourceConfigs,
            _ when actor == Projectile.TargetActor => Definition.targetConfigs,
            _ => Definition.otherConfigs
        };

        foreach (MoveActorConfig config in configs)
            if (config.hookType.HasFlag(type) && actor.TryGetComponent(out CharacterMovement movement))
            {
                Vector3 pushDirection = Projectile.transform.TransformDirection(Vector3.forward + config.direction).normalized;
                movement.ApplyExternalForce(pushDirection * config.forceStrength);
            }
    }

    public override void OnCollide(GameObject target) => MoveActor(target, HookType.OnCollide);

}
