using UnityEngine;

[CreateAssetMenu(fileName = "New Area Damage Behavior", menuName = "Duel/Areas/Damage")]
public class AreaDamageDefinition : AreaBehaviorDefinition
{
    [Tooltip("Period (s) of damage for OnTick.")]
    public float period = 1f;

    [Header("Damage Configs")]
    public DamageConfig[] sourceConfigs;
    public DamageConfig[] targetConfigs;
    public DamageConfig[] otherConfigs;

    public override AreaBehavior CreateRuntimeBehavior(Area area)
    {
        return new AreaDamage(area, this);
    }
}

public class AreaDamage : AreaBehavior
{
    private new AreaDamageDefinition Definition => (AreaDamageDefinition)base.Definition;
    private float _pulseTimer;

    public AreaDamage(Area area, AreaDamageDefinition definition) : base(area, definition) { }

    private DamageConfig[] GetConfigsByActor(GameObject actor)
    {
        if (actor == Area.sourceActor) return Definition.sourceConfigs;
        else if (actor == Area.targetActor) return Definition.targetConfigs;
        else return Definition.otherConfigs;
    }

    private void ApplyDamage(GameObject target, float damage)
    {
        if (target.TryGetComponent(out StatsHandler stats))
            stats.TakeDamage(damage);
    }

    public override void OnTargetEnter(GameObject target)
    {
        foreach (DamageConfig config in GetConfigsByActor(target))
            if (config.hookType.HasFlag(HookType.OnTargetEnter))
                ApplyDamage(target, config.amount);
        
    }
    public override void OnTargetExit(GameObject target)
    {
        foreach (DamageConfig config in GetConfigsByActor(target))
            if (config.hookType.HasFlag(HookType.OnTargetExit))
                ApplyDamage(target, config.amount);
        
    }

    public override void OnTick(float deltaTime)
    {
        _pulseTimer += deltaTime;
        if (_pulseTimer >= Definition.period)
        {
            _pulseTimer = 0f;
            foreach (GameObject target in Area.CurrentTargets)
                foreach (DamageConfig config in GetConfigsByActor(target))
                    if (config.hookType.HasFlag(HookType.OnTick))
                        ApplyDamage(target, config.amount);
        }
    }
    
    public override void OnExpire()
    {
        foreach (GameObject target in Area.CurrentTargets)
            foreach (DamageConfig config in GetConfigsByActor(target))
                if (config.hookType.HasFlag(HookType.OnExpire))
                    ApplyDamage(target, config.amount);
    }
}
