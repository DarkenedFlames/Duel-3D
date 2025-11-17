using UnityEngine;

public class Effect
{
    public EffectDefinition Definition { get; private set; }
    public EffectHandler Handler { get; private set; }

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

        Invoke(Event.OnApply, new PositionContext(){target = Handler.gameObject, localTransform = Handler.transform});
    }

    public float RemainingTime() => _timer;

    bool TryPulse()
    {
        if (_pulse > 0) return false;
        
        _pulse = Definition.period;
        Invoke(Event.OnPulse, new PositionContext(){target = Handler.gameObject, localTransform = Handler.transform});
        return true;
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
        Invoke(Event.OnRefresh, new PositionContext(){target = Handler.gameObject, localTransform = Handler.transform});
    }

    public bool AddStack()
    {
        if (CurrentStacks >= Definition.maxStacks) return false;
        CurrentStacks++;
        if (CurrentStacks > 1)
            Invoke(Event.OnStackGained, new PositionContext(){target = Handler.gameObject, localTransform = Handler.transform});
        return true;
    }

    public bool RemoveStack()
    {
        if (CurrentStacks > 1)
            Invoke(Event.OnStackLost, new PositionContext(){target = Handler.gameObject, localTransform = Handler.transform});

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
                Invoke(Event.OnStackLost, new PositionContext(){target = Handler.gameObject, localTransform = Handler.transform});
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
            Invoke(Event.OnExpire, new PositionContext(){target = Handler.gameObject, localTransform = Handler.transform});
            CurrentStacks = 0;
            return true;
        }
    }

    public void Invoke(Event evt, EventContext ctx)
    {
        foreach (EventReaction r in Definition.reactions)
            if (r.Events.Contains(evt))
                r.OnEvent(ctx);
    }
}
