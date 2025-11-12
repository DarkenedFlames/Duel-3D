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

    public override AreaBehavior CreateRuntimeBehavior(Area area) => new AreaDamage(area, this);
}

public class AreaDamage : AreaBehavior
{
    private new AreaDamageDefinition Definition => (AreaDamageDefinition)base.Definition;
    public AreaDamage(Area area, AreaDamageDefinition definition) : base(area, definition) { }

    private float _pulseTimer;
    private void ApplyDamage(GameObject actor, HookType type)
    {
        if (actor == null) return;

        DamageConfig[] configs = actor switch
        {
            _ when actor == Area.SourceActor => Definition.sourceConfigs,
            _ when actor == Area.TargetActor => Definition.targetConfigs,
            _ => Definition.otherConfigs
        };

        foreach (DamageConfig config in configs)
            if (config.hookType.HasFlag(type) && actor.TryGetComponent(out StatsHandler stats))
                stats.TakeDamage(config.amount);
    }

    public override void OnTargetEnter(GameObject target) => ApplyDamage(target, HookType.OnTargetEnter);
    public override void OnTargetExit(GameObject target) => ApplyDamage(target, HookType.OnTargetExit);

    public override void OnTick(float deltaTime)
    {
        _pulseTimer += deltaTime;
        if (_pulseTimer < Definition.period) return;

        _pulseTimer = 0f;
        foreach (GameObject actor in Area.CurrentTargets)
            ApplyDamage(actor, HookType.OnTick);
    }
    
    public override void OnExpire()
    {
        foreach (GameObject actor in Area.CurrentTargets)
            ApplyDamage(actor, HookType.OnExpire);
    }
}
