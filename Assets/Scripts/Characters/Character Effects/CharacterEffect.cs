using System.Collections.Generic;
using UnityEngine;

public class CharacterEffect : IActionSource
{
    public Character Owner { get; set; }
    public Transform Transform => Owner.transform;
    public GameObject GameObject { get; }
    

    public EffectDefinition Definition;
    public IntegerCounter currentStacks;
    public FloatCounter seconds;
    public FloatCounter pulse;

    public CharacterEffect(GameObject gameObject, EffectDefinition def, int initialStacks)
    {
        GameObject = gameObject;
        if (!gameObject.TryGetComponent(out Character character))
            Debug.LogError($"{gameObject.name}'s {Definition.effectName} expected a {nameof(Character)} but it was missing!");
        else
            Owner = character;
        
        Definition = def;

        currentStacks = new(initialStacks, 0, def.maxStacks, resetToMax: false);
        // Make these two null if their duration/period is not >0, but all references must null check
        seconds = new(def.duration, 0, def.duration);
        pulse = new(def.period, 0, def.period);

        Execute(Definition.OnApplyActions);
        Debug.Log($"{GameObject.name} gained {currentStacks.Value}x {Definition.effectName} ({seconds.Value}s)!");
    }

    public void OnUpdate()
    {
        float dt = Time.deltaTime;
        seconds.Decrease(dt);
        pulse.Decrease(dt);
        
        if (pulse.Expired && Definition.period > 0)
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
        
        int stacksToAdd = Mathf.Min(stacks, Definition.maxStacks - currentStacks.Value);
        if (stacksToAdd <= 0) return 0;

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
            || (Definition.expiryType == ExpiryType.LoseAllStacks && seconds.Expired)
            || (Definition.expiryType == ExpiryType.LoseOneStackAndRefresh && currentStacks.Value <= 1 && seconds.Expired);

        if (shouldExpire)
        {
            Execute(Definition.OnExpireActions);
            Debug.Log($"{GameObject.name} lost {Definition.effectName}!");
            return true;
        }

        if (Definition.expiryType == ExpiryType.LoseOneStackAndRefresh && currentStacks.Value > 1 && seconds.Expired)
        {            
            currentStacks.Decrement();
            seconds.Reset();
        }

        return false;
    }

    void Execute(List<IGameAction> actions)
    {
        float magnitude = 1f;
        if (Definition.ScalesWithStacks)
            magnitude = currentStacks.Value;

        ActionContext context = new() { Source = this, Target = GameObject.GetComponent<Character>(), Magnitude = magnitude };
        actions.ForEach(a => a.Execute(context));
    }
}