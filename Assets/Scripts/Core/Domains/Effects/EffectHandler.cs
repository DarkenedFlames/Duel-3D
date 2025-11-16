using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    public List<Effect> Effects { get; private set; } = new();

    public event Action<Effect> OnEffectApplied;
    public event Action<Effect> OnEffectExpired;
    public event Action<Effect> OnEffectStackChange;
    public event Action<Effect> OnEffectRefreshed;

    public bool TryGetEffect(string effectName, out Effect effect)
    {
        effect = Effects.Find(e => e.Definition.effectName == effectName);
        return effect != null;
    }

    /// <summary>
    /// Applies a new effect or stacks onto an existing one.
    /// </summary>
    public Effect ApplyEffect(EffectDefinition def, int stacks = 1)
    {
        if (stacks <= 0) return null;

        // Already have this effect
        if (TryGetEffect(def.effectName, out Effect existing))
        {
            for (int i = 0; i < stacks; i++)
                ApplyStacking(existing);
            return existing;
        }

        // Create new effect
        var effect = new Effect(def, this);
        Effects.Add(effect);
        OnEffectApplied?.Invoke(effect);

        Debug.Log($"{gameObject.name} has gained {effect.Definition.effectName} for {effect.Definition.duration} seconds.");

        // If more than 1 stack requested, apply the rest
        for (int i = 1; i < stacks; i++)
            ApplyStacking(effect);

        return effect;
    }

    /// <summary>
    /// Called each frame to tick down effect durations.
    /// </summary>
    private void Update()
    {
        for (int i = Effects.Count - 1; i >= 0; i--)
        {
            Effect effect = Effects[i];
            effect.Update();

            if (effect.TryExpire())
            {
                Effects.RemoveAt(i);
                OnEffectExpired?.Invoke(effect);
            }
        }
    }

    public void ApplyStacking(Effect effect)
    {
        bool refreshed = false;
        bool stacked = false;

        if (effect.Definition.stackingType.HasFlag(StackingType.Refresh))
        {
            effect.RefreshTimer();
            refreshed = true;
            OnEffectRefreshed?.Invoke(effect);
        }

        if (effect.Definition.stackingType.HasFlag(StackingType.AddStack))
        {
            if (effect.AddStack())
            {
                stacked = true;
                OnEffectStackChange?.Invoke(effect);
            }
        }

        if (!stacked && !refreshed)
            Debug.Log($"{effect.Definition.effectName} cannot stack or refresh (StackingType.None).");
    }

    /// <summary>
    /// Cleanse an effect instantly without expiration.
    /// </summary>
    public void CleanseEffect(string effectName)
    {
        if (TryGetEffect(effectName, out Effect effect))
        {
            Effects.Remove(effect);
            Debug.Log($"{gameObject.name}'s {effect.Definition.effectName} was cleansed.");
        }
    }

    /// <summary>
    /// Remove a number of stacks from an effect, expiring it if depleted.
    /// </summary>
    public void RemoveStacks(string effectName, int stacks = 1)
    {
        if (TryGetEffect(effectName, out Effect effect))
        {
            for (int i = 0; i < stacks; i++)
            {
                effect.RemoveStack();
                OnEffectStackChange?.Invoke(effect);
                if (effect.TryExpire())
                {
                    Effects.Remove(effect);
                    OnEffectExpired?.Invoke(effect);
                    break;
                }
            }
        }
    }
}
