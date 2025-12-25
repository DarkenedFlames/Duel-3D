using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    public List<EffectDefinition> initialEffects;

    public ReadOnlyCollection<CharacterEffect> Effects => effects.AsReadOnly();
    private readonly List<CharacterEffect> effects = new();

    Character owner;

    public event Action<CharacterEffect> OnEffectGained;
    public event Action<CharacterEffect> OnEffectLost;
    public event Action<CharacterEffect> OnEffectStackChanged;
    public event Action<CharacterEffect> OnEffectRefreshed;
    public event Action<CharacterEffect> OnEffectExtended;
    public event Action<CharacterEffect> OnEffectPulsed;
    public event Action<CharacterEffect> OnEffectMaxStacksReached;

    void Awake() => owner = GetComponent<Character>();

    void Update()
    {
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            CharacterEffect effect = effects[i];
            if (effect.OnUpdate()) OnEffectPulsed?.Invoke(effect);
            
            effect.IsExpired(out bool expired, out bool stacksLost, out bool refreshed);
            if (stacksLost) OnEffectStackChanged?.Invoke(effect);
            if (refreshed) OnEffectRefreshed?.Invoke(effect);
            if (expired)
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

    public void AddEffect(EffectDefinition definition, int stacks, object source)
    {
        if (TryGetEffect(definition, out CharacterEffect existing))
        {
            existing.ApplyStacking(stacks, out bool refreshed, out bool extended, out bool stacksGained, out bool maxStacksReached);
                        
            if (refreshed)        OnEffectRefreshed?.Invoke(existing);
            if (extended)         OnEffectExtended?.Invoke(existing);
            if (stacksGained)     OnEffectStackChanged?.Invoke(existing);
            if (maxStacksReached) OnEffectMaxStacksReached?.Invoke(existing);
        }
        else
        {
            CharacterEffect newEffect = new(owner, definition, stacks, source, out bool maxStacksReached);
            effects.Add(newEffect);
            OnEffectGained?.Invoke(newEffect);
            if (maxStacksReached) OnEffectMaxStacksReached?.Invoke(newEffect);
        }
    }

    public void RemoveEffect(EffectDefinition definition, int stacks)
    {
        if (!TryGetEffect(definition, out CharacterEffect existing)) return;
        
        existing.RemoveStacks(stacks, out bool stacksLost, out bool zeroStacks);
        if (stacksLost) OnEffectStackChanged?.Invoke(existing);
        if (zeroStacks)
        {
            effects.Remove(existing);
            OnEffectLost?.Invoke(existing);
        }
    }

    public void RemoveEffect(EffectDefinition definition)
    {
        if (TryGetEffect(definition, out CharacterEffect existing) && effects.Remove(existing))
            OnEffectLost?.Invoke(existing);
    }
    
    public void RemoveAllEffectsFromSource(object source)
    {
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            CharacterEffect effect = effects[i];
            if (effect.Source == source)
            {
                effects.RemoveAt(i);
                OnEffectLost?.Invoke(effect);
            }
        }
    }
    
    public void RemoveSpecificEffectFromSource(EffectDefinition definition, object source)
    {
	      if (!TryGetEffect(definition, out CharacterEffect effect) || source == null || effect.Source != source) return;
	      
	      RemoveEffect(definition);
    }
    
    public void RemoveSpecificEffectFromSource(EffectDefinition definition, int stacks, object source)
    {
	      if (!TryGetEffect(definition, out CharacterEffect effect) || source == null || effect.Source != source) return;
	      
	      RemoveEffect(definition, stacks);
    }
    
    public void RemoveAllEffects()
    {
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            CharacterEffect effect = effects[i];
            effects.RemoveAt(i);
            OnEffectLost?.Invoke(effect);
        }
    }
}