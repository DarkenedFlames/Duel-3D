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

    public override AreaBehavior CreateRuntimeBehavior(Area area)
    {
        return new AreaModifyStat(area, this);
    }
}

public class AreaModifyStat : AreaBehavior
{
    private new AreaModifyStatDefinition Definition => (AreaModifyStatDefinition)base.Definition;
    private float _pulseTimer;

    public AreaModifyStat(Area area, AreaModifyStatDefinition definition) : base(area, definition) { }

    private StatConfig[] GetConfigsByActor(GameObject actor)
    {
        if (actor == Area.sourceActor) return Definition.sourceConfigs;
        else if (actor == Area.targetActor) return Definition.targetConfigs;
        else return Definition.otherConfigs;
    }

    private void ModifyStat(GameObject target, StatType type, float amount)
    {
        if (target.TryGetComponent(out StatsHandler stats) && stats.TryGetStat(type, out float val))
            stats.TrySetStat(type, val + amount);
    }

    public override void OnTargetEnter(GameObject target)
    {
        foreach (StatConfig config in GetConfigsByActor(target))
            if (config.hookType.HasFlag(HookType.OnTargetEnter))
                ModifyStat(target, config.statType, config.amount);
        
    }
    public override void OnTargetExit(GameObject target)
    {
        foreach (StatConfig config in GetConfigsByActor(target))
            if (config.hookType.HasFlag(HookType.OnTargetExit))
                ModifyStat(target, config.statType, config.amount);
    }

    public override void OnTick(float deltaTime)
    {
        _pulseTimer += deltaTime;
        if (_pulseTimer >= Definition.period)
        {
            _pulseTimer = 0f;
            foreach (GameObject target in Area.CurrentTargets)
                foreach (StatConfig config in GetConfigsByActor(target))
                    if (config.hookType.HasFlag(HookType.OnTick))
                        ModifyStat(target, config.statType, config.amount);
        }
    }
    
    public override void OnExpire()
    {
        foreach (GameObject target in Area.CurrentTargets)
            foreach (StatConfig config in GetConfigsByActor(target))
                if (config.hookType.HasFlag(HookType.OnExpire))
                    ModifyStat(target, config.statType, config.amount);
    }
}
