public class EffectDamage : EffectBehavior<EffectDamageDefinition>
{
    public EffectDamage(EffectDamageDefinition def, Effect owner) : base(def, owner) { }

    private void ApplyDamage(float damage)
    {
        if (Owner.Handler.gameObject.TryGetComponent(out StatsHandler stats)) stats.TakeDamage(damage);
    }

    public override void OnApply() => ApplyDamage(Definition.initialDamage);
    public override void OnUpdate(float deltaTime) => ApplyDamage(Definition.damagePerSecond * deltaTime);
    public override void OnExpire() => ApplyDamage(Definition.expireDamage);
}
