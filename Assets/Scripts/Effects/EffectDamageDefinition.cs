using UnityEngine;

[CreateAssetMenu(fileName = "New Effect Damage Behavior", menuName = "Duel/Effects/Behaviors/Damage")]
public class EffectDamageDefinition : EffectBehaviorDefinition
{

    [Header("Damage Configs")]
    public DamageConfig[] targetConfigs;

    public override EffectBehavior CreateRuntimeBehavior(Effect effect)
    {
        return new EffectDamage(this, effect);
    }
}

public class EffectDamage : EffectBehavior
{
    private new EffectDamageDefinition Definition => (EffectDamageDefinition)base.Definition;

    public EffectDamage(EffectDamageDefinition definition, Effect effect) : base(definition, effect) { }

    private void ApplyDamage(float damage)
    {
        if (Effect.Handler.TryGetComponent(out StatsHandler stats))
            stats.TakeDamage(damage);
    }

    public override void OnApply()
    {
        foreach (DamageConfig config in Definition.targetConfigs)
        {
            if (config.hookType.HasFlag(HookType.OnApply))
            {
                ApplyDamage(config.amount);
            }
        }
    }

    public override void OnTick(float deltaTime)
    {
        foreach (DamageConfig config in Definition.targetConfigs)
        {
            if (config.hookType.HasFlag(HookType.OnTick))
            {
                ApplyDamage(config.amount * deltaTime);
            }
        }
    }
    
    public override void OnExpire()
    {
        foreach (DamageConfig config in Definition.targetConfigs)
        {
            if (config.hookType.HasFlag(HookType.OnExpire))
            {
                ApplyDamage(config.amount);
            }
        }
    }
}
