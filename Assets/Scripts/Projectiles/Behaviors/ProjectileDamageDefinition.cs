using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Damage Behavior", menuName = "Duel/Projectiles/Behaviors/Damage")]
public class ProjectileDamageDefinition : ProjectileBehaviorDefinition
{
    [Header("Damage Configs")]
    public DamageConfig[] sourceConfigs;
    public DamageConfig[] targetConfigs;
    public DamageConfig[] otherConfigs;

    public override ProjectileBehavior CreateRuntimeBehavior(Projectile projectile) => new ProjectileDamage(projectile, this);
}

public class ProjectileDamage : ProjectileBehavior
{
    private new ProjectileDamageDefinition Definition => (ProjectileDamageDefinition)base.Definition;
    public ProjectileDamage(Projectile projectile, ProjectileDamageDefinition definition) : base(projectile, definition) { }

    private void ApplyDamage(GameObject actor, HookType type)
    {
        DamageConfig[] configs = actor switch
        {
            _ when actor == Projectile.SourceActor => Definition.sourceConfigs,
            _ when actor == Projectile.TargetActor => Definition.targetConfigs,
            _ => Definition.otherConfigs
        };

        foreach (DamageConfig config in configs)
            if (config.hookType.HasFlag(type) && actor.TryGetComponent(out StatsHandler stats))
                stats.TakeDamage(config.amount);
    }

    public override void OnCollide(GameObject target) => ApplyDamage(target, HookType.OnCollide);
}
