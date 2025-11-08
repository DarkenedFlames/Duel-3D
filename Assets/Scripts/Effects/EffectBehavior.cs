using UnityEngine;

public abstract class EffectBehavior
{
    public Effect Effect { get; private set; }
    public EffectBehaviorDefinition Definition {get; private set;}

    protected EffectBehavior(EffectBehaviorDefinition def, Effect effect)
    {
        Definition = def;
        Effect = effect;
    }

    public virtual void OnApply() { }
    public virtual void OnTick(float deltaTime) { }
    public virtual void OnExpire() { }
}


