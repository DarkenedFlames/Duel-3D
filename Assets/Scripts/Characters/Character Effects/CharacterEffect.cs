using UnityEngine;

public class CharacterEffect
{
    public EffectDefinition Definition;
    public IntegerCounter currentStacks;
    public FloatCounter seconds;

    public CharacterEffect(EffectDefinition def, int initialStacks)
    {
        Definition = def;
        
        currentStacks = new(initialStacks, 0, def.maxStacks, inclusive: true, resetToMax: false);
        seconds = new(def.duration, 0, def.duration, inclusive: true, resetToMax: true);
    }

    public void OnUpdate()
    {
        seconds.Decrease(Time.deltaTime);
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
        Debug.Log($"Effect {Definition.effectName}: seconds = {seconds.Value}, expired = {seconds.Expired}");

        if (currentStacks.Expired) return true;
        if (!seconds.Expired) return false;
        if (Definition.expiryType == ExpiryType.LoseAllStacks) return true;
        if (Definition.expiryType == ExpiryType.LoseOneStackAndRefresh && currentStacks.Value <= 1) return true;

        if (Definition.expiryType == ExpiryType.LoseOneStackAndRefresh)
        {
            currentStacks.Decrement();
            Refresh();
        }
        return false;
    }
}