using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Area Modify Stat Behavior", menuName = "Duel/Areas/ModifyStat")]
public class AreaModifyStatDefinition : AreaBehaviorDefinition
{
    [Tooltip("Period (s) of stat modification for OnTick.")]
    public float period = 1f;

    [Header("Stat Configs")]
    public StatConfig[] sourceConfigs;
    public StatConfig[] targetConfigs;
    public StatConfig[] otherConfigs;

    public override AreaBehavior CreateRuntimeBehavior(Area area) => new AreaModifyStat(area, this);
}

public class AreaModifyStat : AreaBehavior
{
    private new AreaModifyStatDefinition Definition => (AreaModifyStatDefinition)base.Definition;
    public AreaModifyStat(Area area, AreaModifyStatDefinition definition) : base(area, definition) { }

    private float _pulseTimer;
    private void ModifyStat(GameObject actor, HookType type)
    {
        if (actor == null) return;

        StatConfig[] configs = actor switch
        {
            _ when actor == Area.SourceActor => Definition.sourceConfigs,
            _ when actor == Area.TargetActor => Definition.targetConfigs,
            _ => Definition.otherConfigs
        };

        foreach (StatConfig config in configs)
            if (config.hookType.HasFlag(type) && actor.TryGetComponent(out StatsHandler stats))
                stats.TryModifyStat(config.statType, config.modifyMax, config.amount);
    }

    public override void OnTargetEnter(GameObject target) => ModifyStat(target, HookType.OnTargetEnter);
    public override void OnTargetExit(GameObject target) => ModifyStat(target, HookType.OnTargetExit);

    public override void OnTick(float deltaTime)
    {
        _pulseTimer += deltaTime;
        if (_pulseTimer < Definition.period) return;
        
        _pulseTimer = 0f;
        foreach (GameObject actor in Area.CurrentTargets)
            ModifyStat(actor, HookType.OnTick);
    }
    
    public override void OnExpire()
    {
        foreach (GameObject actor in Area.CurrentTargets)
            ModifyStat(actor, HookType.OnExpire);
    }
}
