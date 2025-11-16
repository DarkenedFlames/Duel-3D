using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Effect
{
    public EffectDefinition Definition { get; private set; }
    public EffectHandler Handler { get; private set; }
    public List<EffectInstructionBinding> bindings;

    private float _timer;
    public int CurrentStacks { get; private set; } = 1;

    private bool ZeroStacks => CurrentStacks <= 0;
    private bool ZeroTime => _timer <= 0;

    private float _pulse;

    public Effect(EffectDefinition definition, EffectHandler handler)
    {
        Definition = definition;
        Handler = handler;
        _timer = definition.duration;
        _pulse = definition.period;
        bindings = definition.bindings;

        PerformInstruction(EffectHook.OnApply);
    }

    public float RemainingTime() => _timer;

    bool TryPulse()
    {
        if (_pulse <= 0)
        {
            _pulse = Definition.period;
            PerformInstruction(EffectHook.OnPulse);
            return true;
        }
        return false;
    }

    public void Update()
    {
        float dt = Time.deltaTime;

        _pulse -= dt;
        TryPulse();

        _timer -= dt;
        TryExpire();
    }

    public void RefreshTimer()
    {
        _timer = Definition.duration;
        PerformInstruction(EffectHook.OnRefresh);
    }

    public bool AddStack()
    {
        if (CurrentStacks >= Definition.maxStacks) return false;
        CurrentStacks++;
        if (CurrentStacks > 1)
            PerformInstruction(EffectHook.OnStackGained);
        return true;
    }

    public bool RemoveStack()
    {
        if (CurrentStacks > 1)
            PerformInstruction(EffectHook.OnStackLost);

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
                PerformInstruction(EffectHook.OnStackLost);
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
            PerformInstruction(EffectHook.OnExpire);
            CurrentStacks = 0;
            return true;
        }
    }

    void PerformInstruction(EffectHook hook)
    {
        EffectContext effectContext = new(Handler, Handler.gameObject);

        foreach (EffectInstructionBinding binding in bindings)
            if (binding.Hook.Equals(hook))
                binding.Execute(effectContext);
    }
}
