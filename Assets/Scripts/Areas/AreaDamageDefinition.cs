using UnityEngine;

[CreateAssetMenu(fileName = "New Area Damage Behavior", menuName = "Duel/Areas/Behaviors/Damage")]
public class AreaDamageDefinition : AreaBehaviorDefinition
{

    [Header("Damage Configs")]
    public DamageConfig[] sourceConfigs;
    public DamageConfig[] targetConfigs;

    public override AreaBehavior CreateRuntimeBehavior(Area area)
    {
        return new AreaDamage(area, this);
    }
}

public class AreaDamage : AreaBehavior
{
    private new AreaDamageDefinition Definition => (AreaDamageDefinition)base.Definition;

    public AreaDamage(Area area, AreaDamageDefinition definition) : base(area, definition) { }

    private void ApplyDamage(GameObject target, float damage)
    {
        if (target.TryGetComponent(out StatsHandler stats))
            stats.TakeDamage(damage);
    }

    public override void OnTargetEnter(Collider target)
    {
        foreach (DamageConfig config in Definition.targetConfigs)
        {
            if (config.hookType.HasFlag(HookType.OnTargetEnter))
            {
                ApplyDamage(target.gameObject, config.amount);
            }
        }
    }
    public override void OnTargetExit(Collider target)
    {
        foreach (DamageConfig config in Definition.targetConfigs)
        {
            if (config.hookType.HasFlag(HookType.OnTargetExit))
            {
                ApplyDamage(target.gameObject, config.amount);
            }
        }
    }

    public override void OnTick(float deltaTime)
    {
        foreach (DamageConfig config in Definition.targetConfigs)
        {
            if (config.hookType.HasFlag(HookType.OnTick))
            { 
                foreach (GameObject target in Area.CurrentTargets)
                {
                    if (target != Area.sourceActor)
                    {
                        ApplyDamage(target, config.amount * deltaTime);
                    }
                }
            }
        }
    }
    
    // can do on expire as well
}
