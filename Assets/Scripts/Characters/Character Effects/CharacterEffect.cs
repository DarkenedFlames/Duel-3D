using UnityEngine;

public class CharacterEffect : IActionSource
{
    public Character Owner { get; set; }
    public Transform Transform => Owner.transform;
    public GameObject GameObject => Owner.gameObject;
    public object Source;

    public EffectDefinition Definition;
    public IntegerCounter currentStacks;
    public FloatCounter seconds;
    public FloatCounter pulse;
    
    public float CurrentDuration;

    public CharacterEffect(Character owner, EffectDefinition def, int initialStacks, object source, out bool maxStacksReached)
    {
        Owner = owner;
        Definition = def;
        Source = source;

        currentStacks = new(initialStacks, 0, def.maxStacks, resetToMax: false);
        maxStacksReached = currentStacks.Exceeded;
        
        if (maxStacksReached)
            Execute(EffectHook.OnMaxStackReached);
            
        if (def.duration > 0)
        {
            CurrentDuration = def.duration;
            seconds = new(def.duration, 0, def.duration);
        }
        if (def.period > 0)
            pulse = new(def.period, 0, def.period);
        
        Execute(EffectHook.OnApply);
        
        for (int i = 0; i < currentStacks.Value; i++)
            Execute(EffectHook.OnStackGained, false);
    }

    public bool OnUpdate()
    {
        float dt = Time.deltaTime;
        seconds?.Decrease(dt);
        pulse?.Decrease(dt);
        
        if (pulse != null && pulse.Expired)
        {
            Execute(EffectHook.OnPulse);
            pulse.Reset();
            return true;
        }
        return false;
    }

    public void ApplyStacking(int stacks, out bool refreshed, out bool extended, out bool stacksGained, out bool maxStacksReached)
    {
        refreshed = false;
        extended = false;
        stacksGained = false;
        maxStacksReached = false;
    
        switch (Definition.EffectStackingType)
        {
            case EffectStackingType.Ignore: break;
            case EffectStackingType.Refresh: break;
            case EffectStackingType.AddStackAndRefresh:
                int stacksToAdd = Mathf.Clamp(stacks, 0, Definition.maxStacks - currentStacks.Value);
                if (stacksToAdd > 0)
                {
                    currentStacks.Increase(stacksToAdd);
                    for (int i = 0; i < stacksToAdd; i++)
                        Execute(EffectHook.OnStackGained, false);
                    stacksGained = true;
                    maxStacksReached = currentStacks.Exceeded;
                    if (maxStacksReached)
                        Execute(EffectHook.OnMaxStackReached);    
                }
                break;
            case EffectStackingType.ExtendDuration:
                if (seconds != null)
                {
                    CurrentDuration += Definition.duration;
                    seconds.SetMax(CurrentDuration);
                    extended = true;
                    Execute(EffectHook.OnExtended);
                }
                break;
        }
        if (seconds == null || Definition.EffectStackingType == EffectStackingType.Ignore) return;
        
        seconds.Reset();
        refreshed = true;
        Execute(EffectHook.OnRefreshed);
    }
    
    public void RemoveStacks(int stacks, out bool stacksLost, out bool zeroStacks)
    {
        stacksLost = false;
        zeroStacks = false;
        
        int stacksToRemove = Mathf.Clamp(stacks, 0, currentStacks.Value);
        if (stacksToRemove > 0)
        {
            currentStacks.Decrease(stacksToRemove);
            for (int i = 0; i < stacksToRemove; i++)
                Execute(EffectHook.OnStackLost, false);
            stacksLost = true;
            zeroStacks = currentStacks.Expired;
        }
    }

    public void IsExpired(out bool expired, out bool stacksLost, out bool refreshed)
    {
        expired = false;
        refreshed = false;
        stacksLost = false;
        
        bool hasExpiredDuration = seconds != null && seconds.Expired;
    
        bool shouldExpire = currentStacks.Expired
            || (hasExpiredDuration && Definition.expiryType == ExpiryType.LoseAllStacks)
            || (hasExpiredDuration && Definition.expiryType == ExpiryType.LoseOneStackAndRefresh && currentStacks.Value <= 1);

        if (shouldExpire)
        {
            if (!currentStacks.Expired)
            {
                for (int i = 0; i < currentStacks.Value; i++)
                    Execute(EffectHook.OnStackLost, false);
            }

            stacksLost = !currentStacks.Expired;
            Execute(EffectHook.OnRemove);
            expired = true;
            return;
        }

        if (hasExpiredDuration && Definition.expiryType == ExpiryType.LoseOneStackAndRefresh && currentStacks.Value > 1)
        {            
            currentStacks.Decrement();
            seconds.Reset();
            stacksLost = true;
            refreshed = true;
            Execute(EffectHook.OnStackLost, false);
            Execute(EffectHook.OnRefreshed);
        }
    }

    void Execute(EffectHook hook, bool scalesWithStacks = true)
    {
        float magnitude = 1f;
        if (Definition.ScalesWithStacks && scalesWithStacks)
            magnitude = currentStacks.Value;

        ActionContext context = new() { Source = this, Target = Owner, Magnitude = magnitude };
        Definition.ExecuteActions(hook, context);
    }
}