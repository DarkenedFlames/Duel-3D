using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Flags]
public enum StackingType
{
    None = 0,
    AddStack = 1 << 0,
    Refresh = 1 << 1
}

public enum ExpiryType
{
    LoseOneStackAndRefresh,
    LoseAllStacks
}

public class EffectHandler : MonoBehaviour
{
    public List<Effect> Effects { get; private set; } = new();

    public bool HasEffect(string effectName) =>
        Effects.Any(e => e.Definition.effectName == effectName);

    public bool TryGetEffect(string effectName, out Effect effect)
    {
        effect = Effects.Find(e => e.Definition.effectName == effectName);
        return effect != null;
    }


    /// <summary>
    /// Apply a certain number of stacks of an effect (according to StackingType)
    /// </summary>
    public void ApplyEffect(EffectDefinition def, int stacks = 1)
    {
        if (stacks <= 0) return;

        if (TryGetEffect(def.effectName, out Effect existing))
        {
            for (int i = 0; i < stacks; i++)
                existing.ApplyStacking();
        }
        else
        {
            var effect = new Effect(def, this);
            Effects.Add(effect);
            Debug.Log($"{gameObject.name} has gained {effect.Definition.effectName} for {effect.Definition.duration} seconds.");

            // If more than 1 stack requested, apply the rest as stack gains
            for (int i = 1; i < stacks; i++)
                effect.ApplyStacking();
        }
    }

    /// <summary>
    /// Tick effects, expiring each one if it IsExpired()
    /// </summary>
    private void Update()
    {
        float dt = Time.deltaTime;
        for (int i = Effects.Count - 1; i >= 0; i--)
        {
            Effect effect = Effects[i];
            effect.Tick(dt);

            if (effect.TryExpire())
                Effects.RemoveAt(i);
        }
    }

    /// <summary>
    /// Force an effect to expire by name, ignoring whether it IsExpired()
    /// </summary>
    public void ForceExpire(string effectName)
    {
        if (TryGetEffect(effectName, out Effect effect))
        {
            effect.Expire();
            Effects.Remove(effect);   
        }
    }

    /// <summary>
    /// Remove stacks from an effect by name.
    /// </summary>
    public void RemoveStacks(string effectName, int stacks = 1)
    {
        if (TryGetEffect(effectName, out Effect effect))
        {
            for (int i = 0; i < stacks; i++)
            {
                effect.RemoveStack();
                if (effect.TryExpire())
                {
                    Effects.Remove(effect);
                    break;
                }
            }           
        }
    }
}