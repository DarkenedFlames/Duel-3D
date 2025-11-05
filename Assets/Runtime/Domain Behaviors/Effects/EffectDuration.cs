public class EffectDuration : EffectBehavior<EffectDurationDefinition>
{
    public EffectDuration(EffectDurationDefinition def, Effect owner) : base(def, owner) { }

    private float remainingDuration;

    public override void OnApply() => remainingDuration = Definition.duration;
    
    public override void OnUpdate(float deltaTime)
    {
        remainingDuration -= deltaTime;
        if (remainingDuration <= 0f) Owner.Handler.Expire(Owner);
    }
}
