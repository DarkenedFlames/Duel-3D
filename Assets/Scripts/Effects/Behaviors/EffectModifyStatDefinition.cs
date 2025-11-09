using UnityEngine;

[CreateAssetMenu(fileName = "New Effect Modify Stat Behavior", menuName = "Duel/Effects/Behaviors/Modify Stat")]
public class EffectModifyStatDefinition : EffectBehaviorDefinition
{
    [Tooltip("Period (s) of stat modification for OnTick.")]
    public float period = 1f;

    [Header("Stat Configs")]
    public StatConfig[] targetConfigs;

    public override EffectBehavior CreateRuntimeBehavior(Effect effect)
    {
        return new EffectModifyStat(this, effect);
    }
}

public class EffectModifyStat : EffectBehavior
{
    private new EffectModifyStatDefinition Definition => (EffectModifyStatDefinition)base.Definition;
    private float _pulseTimer;

    public EffectModifyStat(EffectModifyStatDefinition definition, Effect effect) : base(definition, effect) { }

    private void ModifyStat(StatType type, float amount)
    {
        if (Effect.Handler.TryGetComponent(out StatsHandler stats))
            stats.TryDecreaseStat(type, amount); // Need to make modify logic
    }

    public override void OnApply()
    {
        foreach (StatConfig config in Definition.targetConfigs)
            if (config.hookType.HasFlag(HookType.OnApply))
                ModifyStat(config.statType, config.amount);
    }

    public override void OnTick(float deltaTime)
    {
        _pulseTimer += deltaTime;
        if (_pulseTimer >= Definition.period)
        {
            _pulseTimer = 0f;
            foreach (StatConfig config in Definition.targetConfigs)
                if (config.hookType.HasFlag(HookType.OnTick))
                    ModifyStat(config.statType, config.amount);
        }
    }
    
    public override void OnExpire()
    {
        foreach (StatConfig config in Definition.targetConfigs)
            if (config.hookType.HasFlag(HookType.OnExpire))
                ModifyStat(config.statType, config.amount);
    }
}
