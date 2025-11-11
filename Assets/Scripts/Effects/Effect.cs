using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Effect
{
    public EffectDefinition Definition { get; private set; }
    private readonly List<EffectBehavior> Behaviors;
    public EffectHandler Handler { get; private set; }

    private float _timer;
    public int CurrentStacks { get; private set; } = 1;

    private bool ZeroStacks => CurrentStacks <= 0;
    private bool ZeroTime => _timer <= 0;

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

    public float RemainingTime() => _timer;

    public bool Tick(float deltaTime)
    {
        Behaviors.ForEach(b => b.OnTick(deltaTime));
        _timer -= deltaTime;
        return TryExpire();
    }

    public void RefreshTimer()
    {
        _timer = Definition.duration;
        Behaviors.ForEach(b => b.OnRefresh());
    }

    public bool AddStack()
    {
        if (CurrentStacks >= Definition.maxStacks) return false;
        CurrentStacks++;
        if (CurrentStacks > 1)
            Behaviors.ForEach(b => b.OnStackGained());
        return true;
    }

    public bool RemoveStack()
    {
        if (CurrentStacks > 1)
            Behaviors.ForEach(b => b.OnStackLost());

        int newCount = Mathf.Max(0, CurrentStacks - 1);
        bool changed = newCount != CurrentStacks;
        CurrentStacks = newCount;
        return changed;
    }

    /// <summary>
    /// Attempts to expire the effect. Returns true if the effect expired.
    /// </summary>
    public bool TryExpire()
    {
        // Lose-one-stack behavior is special: we only partially expire
        if (Definition.expiryType == ExpiryType.LoseOneStackAndRefresh && ZeroTime)
        {
            if (CurrentStacks > 1)
            {
                CurrentStacks--;
                RefreshTimer();
                Behaviors.ForEach(b => b.OnStackLost());
                return false;
            }
        }

        // General expiry conditions
        bool shouldExpire = ZeroStacks
            || (ZeroTime && Definition.expiryType == ExpiryType.LoseAllStacks)
            || (ZeroTime && Definition.expiryType == ExpiryType.LoseOneStackAndRefresh && CurrentStacks <= 1);

        // Expire effect if it should expire
        if (!shouldExpire)
            return false;
        else
        {
            Debug.Log($"{Handler.gameObject.name}'s {Definition.effectName} effect has expired.");
            Behaviors.ForEach(b => b.OnExpire());
            CurrentStacks = 0;
            return true;
        }
    }
}
