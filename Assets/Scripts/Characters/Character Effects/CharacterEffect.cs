using System.Collections.Generic;
using UnityEngine;

public class CharacterEffect
{
    public GameObject GameObject;
    public EffectDefinition Definition;
    public IntegerCounter currentStacks;
    public FloatCounter seconds;
    public FloatCounter pulse;

    public CharacterEffect(GameObject gameObject, EffectDefinition def, int initialStacks)
    {
        GameObject = gameObject;
        Definition = def;

        currentStacks = new(initialStacks, 0, def.maxStacks, resetToMax: false);
        seconds = new(def.duration, 0, def.duration);
        pulse = new(def.period, 0, def.period);

        Execute(Definition.OnApplyActions);
    }

    public void OnUpdate()
    {
        float dt = Time.deltaTime;
        seconds.Decrease(dt);
        pulse.Decrease(dt);
        
        if (pulse.Expired)
        {
            Execute(Definition.OnPulseActions);
            pulse.Reset();
        }
    }

    public bool Refresh()
    {
        if (!Definition.stackingType.HasFlag(StackingType.Refresh)) return false;
        seconds.Reset();
        return true;
    }

    public int AddStacks(int stacks)
    {
        if (stacks <= 0) return 0;
        if (!Definition.stackingType.HasFlag(StackingType.AddStack)) return 0;
        
        int stacksToAdd = Mathf.Min(currentStacks.Value + stacks, Definition.maxStacks);
        currentStacks.Increase(stacksToAdd);
        return stacksToAdd;
    }

    public int RemoveStacks(int stacks)
    {
        if (stacks <= 0) return 0;
        
        int stacksToRemove = Mathf.Max(0, currentStacks.Value - stacks);
        currentStacks.Decrease(stacksToRemove);
        return stacksToRemove;
    }

    public bool TryExpire()
    {
        bool shouldExpire = currentStacks.Expired
            || Definition.expiryType == ExpiryType.LoseAllStacks && seconds.Expired
            || Definition.expiryType == ExpiryType.LoseOneStackAndRefresh && currentStacks.Value <= 1 && seconds.Expired;

        if (shouldExpire)
        {
            Execute(Definition.OnExpireActions);
            return true;
        }
        else if (Definition.expiryType == ExpiryType.LoseOneStackAndRefresh)
        {
            currentStacks.Decrement();
            seconds.Reset();
        }
        return false;
    }

    void Execute(List<IGameAction> actions) => actions.ForEach(a => a.Execute(GameObject, GameObject));
}