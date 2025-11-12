using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Modify Effect Behavior", menuName = "Duel/Projectiles/Behaviors/ModifyEffect")]
public class ProjectileModifyEffectDefinition : ProjectileBehaviorDefinition
{
    [Header("Effect Configs")]
    public EffectConfig[] sourceConfigs;
    public EffectConfig[] targetConfigs;
    public EffectConfig[] otherConfigs;

    public override ProjectileBehavior CreateRuntimeBehavior(Projectile projectile) =>
         new ProjectileModifyEffect(projectile, this);
}

public class ProjectileModifyEffect : ProjectileBehavior
{
    private new ProjectileModifyEffectDefinition Definition => (ProjectileModifyEffectDefinition)base.Definition;
    public ProjectileModifyEffect(Projectile projectile, ProjectileModifyEffectDefinition definition) : base(projectile, definition) { }
    
    private void ModifyEffect(GameObject actor, HookType type)
    {
        EffectConfig[] configs = actor switch
        {
            _ when actor == Projectile.SourceActor => Definition.sourceConfigs,
            _ when actor == Projectile.TargetActor => Definition.targetConfigs,
            _ => Definition.otherConfigs
        };

        foreach (EffectConfig config in configs)
            if (config.hookType.HasFlag(type))
                if (actor.TryGetComponent(out EffectHandler handler))
                    if (config.mode)
                        handler.ApplyEffect(config.effectDefinition, config.stacks);
                    else
                        handler.RemoveStacks(config.effectDefinition.effectName, config.stacks);
    }

    public override void OnCollide(GameObject target) => ModifyEffect(target, HookType.OnCollide);
}
