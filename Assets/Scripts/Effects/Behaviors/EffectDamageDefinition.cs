using UnityEngine;

[CreateAssetMenu(fileName = "New Effect Damage Behavior", menuName = "Duel/Effects/Behaviors/Damage")]
public class EffectDamageDefinition : EffectBehaviorDefinition
{
    [Tooltip("Period (s) of damage for OnTick.")]
    public float period = 1f;

    [Header("Damage Configs")]
    public DamageConfig[] targetConfigs;

    public override EffectBehavior CreateRuntimeBehavior(Effect effect) => new EffectDamage(this, effect);
}

public class EffectDamage : EffectBehavior
{
    private new EffectDamageDefinition Definition => (EffectDamageDefinition)base.Definition;
    public EffectDamage(EffectDamageDefinition definition, Effect effect) : base(definition, effect) { }

    private float _pulseTimer;
    private void ApplyDamage(HookType type)
    {
        foreach (DamageConfig config in Definition.targetConfigs)
            if (config.hookType.HasFlag(type) && Effect.Handler.TryGetComponent(out StatsHandler stats))
                stats.TakeDamage(config.amount);
    }

    public override void OnApply() => ApplyDamage(HookType.OnApply);
    public override void OnStackGained() => ApplyDamage(HookType.OnStackGained);
    public override void OnRefresh() => ApplyDamage(HookType.OnRefresh);

    public override void OnTick(float deltaTime)
    {
        _pulseTimer += deltaTime;
        if (_pulseTimer < Definition.period) return;

        _pulseTimer = 0f;
        ApplyDamage(HookType.OnTick);
    }

    public override void OnStackLost() => ApplyDamage(HookType.OnStackLost);
    public override void OnExpire() => ApplyDamage(HookType.OnExpire);
}
