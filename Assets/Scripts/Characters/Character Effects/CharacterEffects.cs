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

    public bool TryGetEffect(string effectName, out CharacterEffect effect)
    {
        effect = effects.Find(e => e.Definition.effectName == effectName);
        return effect != null;
    }


    // Returns the number of stacks added.
    public int AddEffect(EffectDefinition effectDefinition, int stacks)
    {
        if (TryGetEffect(effectDefinition.effectName, out CharacterEffect existing))
        {
            if (existing.Refresh()) OnEffectRefreshed?.Invoke(existing);

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
            CharacterEffect newEffect = new(gameObject, effectDefinition, stacks);
            effects.Add(newEffect);
            OnEffectGained?.Invoke(newEffect);
            Debug.Log($"{gameObject.name} gained {newEffect.Definition.effectName}");
            return stacks;
        }
    }

    // Returns the number of stacks removed.
    public int RemoveEffect(EffectDefinition effectDefinition, int stacksToRemove)
    {
        if (!TryGetEffect(effectDefinition.effectName, out CharacterEffect existing))
            return 0;
        
        OnEffectStackChanged?.Invoke(existing);
        return existing.RemoveStacks(stacksToRemove);
    }
}