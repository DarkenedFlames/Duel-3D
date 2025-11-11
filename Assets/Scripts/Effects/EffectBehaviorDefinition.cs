using UnityEngine;

public abstract class EffectBehaviorDefinition : ScriptableObject
{
    public abstract EffectBehavior CreateRuntimeBehavior(Effect owner);
}

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
    public virtual void OnStackGained() { }
    public virtual void OnRefresh() { }
    public virtual void OnTick(float deltaTime) { }
    public virtual void OnStackLost() { }
    public virtual void OnExpire() { }
}


