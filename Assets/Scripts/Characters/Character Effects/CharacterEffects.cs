using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    public List<EffectDefinition> initialEffects;

    public ReadOnlyCollection<CharacterEffect> Effects => effects.AsReadOnly();
    private readonly List<CharacterEffect> effects = new();

    public event Action<CharacterEffect> OnEffectGained;
    public event Action<CharacterEffect> OnEffectLost;
    public event Action<CharacterEffect> OnEffectStackChanged;
    public event Action<CharacterEffect> OnEffectRefreshed;

    void Awake()
    {
        foreach (EffectDefinition definition in initialEffects)
            AddEffect(definition, 1);
    }

    void Update()
    {
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            CharacterEffect effect = effects[i];
            effect.OnUpdate();

            if (effect.TryExpire())
            {
                effects.RemoveAt(i);
                OnEffectLost?.Invoke(effect);
            }
        }
    }

    public bool TryGetEffect(EffectDefinition definition, out CharacterEffect effect)
    {
        effect = effects.Find(e => e.Definition == definition);
        return effect != null;
    }


    // Returns the number of stacks added.
    public int AddEffect(EffectDefinition definition, int stacks)
    {
        if (TryGetEffect(definition, out CharacterEffect existing))
        {
            if (existing.Refresh())
                OnEffectRefreshed?.Invoke(existing);

            int stacksGained = existing.AddStacks(stacks);
            if (stacksGained > 0)
            {
                OnEffectStackChanged?.Invoke(existing);
                return stacksGained;
            }

            return 0;
        }
        else
        {
            CharacterEffect newEffect = new(gameObject, definition, stacks);
            effects.Add(newEffect);
            OnEffectGained?.Invoke(newEffect);
            return stacks;
        }
    }

    // Returns the number of stacks removed.
    public int RemoveEffect(EffectDefinition definition, int stacksToRemove)
    {
        if (!TryGetEffect(definition, out CharacterEffect existing))
            return 0;
        
        OnEffectStackChanged?.Invoke(existing);
        return existing.RemoveStacks(stacksToRemove);
    }

    public bool RemoveEffect(EffectDefinition definition)
    {
        if (!TryGetEffect(definition, out CharacterEffect existing) || !effects.Remove(existing))
            return false;

        OnEffectLost?.Invoke(existing);
        return true;
    }
}