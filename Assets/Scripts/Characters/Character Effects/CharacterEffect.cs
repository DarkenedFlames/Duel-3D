using System.Collections.Generic;
using UnityEngine;

public class CharacterEffect : IActionSource
{
    public Character Owner { get; set; }
    public Transform Transform => Owner.transform;
    public GameObject GameObject => Owner.gameObject;
    public object Source;

    public EffectDefinition Definition;
    public IntegerCounter currentStacks;
    public FloatCounter seconds; // null if duration <= 0, reference must null check
    public FloatCounter pulse;  // null if period <= 0, references must null check
    
    public float CurrentDuration; // null if duration <= 0, references must null check

    public CharacterEffect(Character owner, EffectDefinition def, int initialStacks, object source, out bool maxStacksReached)
    {
        Owner = owner;
        Definition = def;
        Source = source;

        currentStacks = new(initialStacks, 0, def.maxStacks, resetToMax: false);
        maxStacksReached = currentStacks.Exceeded;
        
        if (maxStacksReached)
	        Execute(Definition.OnMaxStackReachedActions);
            
        if (def.duration > 0)
        {
            CurrentDuration = def.duration;
            seconds = new(def.duration, 0, def.duration);
        }
        if (def.period > 0)
            pulse = new(def.period, 0, def.period);
        
        Execute(Definition.OnApplyActions);
        
        for (int i = 0; i < currentStacks.Value; i++)
	        Execute(Definition.OnStackGainedActions, false);
	          
        Debug.Log($"{GameObject.name} gained {currentStacks.Value}x {Definition.effectName} ({seconds.Value}s)!");
    }

    public bool OnUpdate()
    {
        float dt = Time.deltaTime;
        seconds?.Decrease(dt);
        pulse?.Decrease(dt);
        
        if (pulse != null && pulse.Expired)
        {
            Execute(Definition.OnPulseActions);
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
	                    Execute(Definition.OnStackGainedActions, false);
                    stacksGained = true;
                    maxStacksReached = currentStacks.Exceeded;
                    if (maxStacksReached)
						Execute(Definition.OnMaxStackReachedActions);    
                }
                break;
            case EffectStackingType.ExtendDuration:
                if (seconds != null)
                {
                    CurrentDuration += Definition.duration;
                    seconds.SetMax(CurrentDuration);
                    extended = true;
					Execute(Definition.OnExtendedActions);
                }
                break;
        }
        if (seconds == null || Definition.EffectStackingType == EffectStackingType.Ignore) return;
        
        seconds.Reset();
        refreshed = true;
        Execute(Definition.OnRefreshedActions);
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
                Execute(Definition.OnStackLostActions, false);
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
                    Execute(Definition.OnStackLostActions, false);
            }

            stacksLost = !currentStacks.Expired;
            Execute(Definition.OnExpireActions);
            expired = true;
            Debug.Log($"{GameObject.name} lost {Definition.effectName}!");
            return;
        }

        if (hasExpiredDuration && Definition.expiryType == ExpiryType.LoseOneStackAndRefresh && currentStacks.Value > 1)
        {            
            currentStacks.Decrement();
            seconds.Reset();
            stacksLost = true;
            refreshed = true;
            Execute(Definition.OnStackLostActions, false);
            Execute(Definition.OnRefreshedActions);
        }
    }

    void Execute(List<IGameAction> actions, bool scalesWithStacks = true)
    {
        float magnitude = 1f;
        if (Definition.ScalesWithStacks && scalesWithStacks)
            magnitude = currentStacks.Value;

        ActionContext context = new() { Source = this, Target = Owner, Magnitude = magnitude };
        actions.ForEach(a => a.Execute(context));
    }
}