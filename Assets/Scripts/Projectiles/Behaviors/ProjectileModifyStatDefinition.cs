using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Modify Stat Behavior", menuName = "Duel/Projectiles/Behaviors/ModifyStat")]
public class ProjectileModifyStatDefinition : ProjectileBehaviorDefinition
{

    [Header("Stat Configs")]
    public StatConfig[] sourceConfigs;
    public StatConfig[] targetConfigs;
    public StatConfig[] otherConfigs;

    public override ProjectileBehavior CreateRuntimeBehavior(Projectile projectile) => new ProjectileModifyStat(projectile, this);
}

public class ProjectileModifyStat : ProjectileBehavior
{
    private new ProjectileModifyStatDefinition Definition => (ProjectileModifyStatDefinition)base.Definition;
    public ProjectileModifyStat(Projectile projectile, ProjectileModifyStatDefinition definition) : base(projectile, definition) { }

    private void ModifyStat(GameObject actor, HookType type)
    {
        StatConfig[] configs = actor switch
        {
            _ when actor == Projectile.SourceActor => Definition.sourceConfigs,
            _ when actor == Projectile.TargetActor => Definition.targetConfigs,
            _ => Definition.otherConfigs
        };

        foreach (StatConfig config in configs)
            if (config.hookType.HasFlag(type) && actor.TryGetComponent(out StatsHandler stats))
                stats.TryModifyStat(config.statType, config.modifyMax, config.amount);
    }

    public override void OnCollide(GameObject target) => ModifyStat(target, HookType.OnCollide);

}
