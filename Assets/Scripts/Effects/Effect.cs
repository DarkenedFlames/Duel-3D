using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Effect
{
    public EffectDefinition Definition { get; private set; }
    public List<EffectBehavior> Behaviors { get; private set; }
    public EffectHandler Handler { get; private set; }

    private float _timer;
    public int CurrentStacks { get; private set; } = 1;

    public bool ZeroStacks => CurrentStacks <= 0;
    public bool ZeroTime => _timer <= 0;

    public Effect(EffectDefinition definition, EffectHandler handler)
    {
        Definition = definition;
        Handler = handler;
        _timer = definition.duration;

        Behaviors = definition.behaviors
            .Select(b => b.CreateRuntimeBehavior(this))
            .ToList();

        Behaviors.ForEach(b => b.OnApply());
    }

    public bool Tick(float deltaTime)
    {
        Behaviors.ForEach(b => b.OnTick(deltaTime));
        _timer -= deltaTime;
        return TryExpire();
    }

    private void RefreshTimer()
    {
        _timer = Definition.duration;
        Behaviors.ForEach(b => b.OnRefresh());
    }

    private bool AddStack()
    {
        if (CurrentStacks >= Definition.maxStacks) return false;
        CurrentStacks++;
        if (CurrentStacks > 1)
            Behaviors.ForEach(b => b.OnStackGained());
        return true;
    }

    public void ApplyStacking()
    {
        if (Definition.stackingType.HasFlag(StackingType.Refresh)) RefreshTimer();
        if (Definition.stackingType.HasFlag(StackingType.AddStack)) AddStack();
    }

    public void RemoveStack()
    {
        if (CurrentStacks > 1)
            Behaviors.ForEach(b => b.OnStackLost());

        CurrentStacks = Mathf.Max(0, CurrentStacks - 1);
    }

    /// <summary>
    /// Checks if the effect should expire without expiring it.
    /// </summary>
    public bool IsExpired()
    {
        if (ZeroStacks) return true;
        if (Definition.expiryType == ExpiryType.LoseAllStacks && ZeroTime) return true;
        if (Definition.expiryType == ExpiryType.LoseOneStackAndRefresh && ZeroTime) return true;
        return false;
    }

    /// <summary>
    /// Attempts to expire the effect. Returns true if the effect expired.
    /// </summary>
    public bool TryExpire()
    {
        if (!IsExpired()) return false;

        if (Definition.expiryType == ExpiryType.LoseOneStackAndRefresh && ZeroTime && CurrentStacks > 1)
        {
            CurrentStacks--;
            RefreshTimer();
            Behaviors.ForEach(b => b.OnStackLost());
            return false;
        }

        Expire();
        return true;
    }

    public void Expire()
    {
        Debug.Log($"{Handler.gameObject.name}'s {Definition.effectName} effect has expired.");
        Behaviors.ForEach(b => b.OnExpire());
        CurrentStacks = 0;
    }
}
