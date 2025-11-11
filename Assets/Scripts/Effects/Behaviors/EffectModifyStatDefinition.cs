using UnityEngine;

[CreateAssetMenu(fileName = "New Effect Modify Stat Behavior", menuName = "Duel/Effects/Behaviors/ModifyStat")]
public class EffectModifyStatDefinition : EffectBehaviorDefinition
{
    [Tooltip("Period (s) of stat modification for OnTick.")]
    public float period = 1f;

    [Header("Stat Configs")]
    public StatConfig[] targetConfigs;

    public override EffectBehavior CreateRuntimeBehavior(Effect effect) => new EffectModifyStat(this, effect);
}

public class EffectModifyStat : EffectBehavior
{
    private new EffectModifyStatDefinition Definition => (EffectModifyStatDefinition)base.Definition;
    public EffectModifyStat(EffectModifyStatDefinition definition, Effect effect) : base(definition, effect) { }

    private float _pulseTimer;
    private void ModifyStat(HookType type)
    {
       foreach (StatConfig config in Definition.targetConfigs)
            if (config.hookType.HasFlag(type) && Effect.Handler.TryGetComponent(out StatsHandler stats))
                stats.TryModifyStat(config.statType, config.modifyMax, config.amount);
    }

    public override void OnApply() => ModifyStat(HookType.OnApply);
    public override void OnStackGained() => ModifyStat(HookType.OnStackGained);
    public override void OnRefresh() => ModifyStat(HookType.OnRefresh);
    public override void OnTick(float deltaTime)
    {
        _pulseTimer += deltaTime;
        if (_pulseTimer < Definition.period) return;

        _pulseTimer = 0f;
        ModifyStat(HookType.OnTick);
    }
    public override void OnStackLost() => ModifyStat(HookType.OnStackLost);
    public override void OnExpire() => ModifyStat(HookType.OnTick);
}
