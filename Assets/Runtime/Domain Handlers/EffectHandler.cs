using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatsHandler))]
public class EffectHandler : MonoBehaviour
{
    readonly List<Effect> Effects = new();

    void Update()
    {
        float dt = Time.deltaTime;
        Effects.ForEach(e =>
            {
                e.GetAllGates().ForEach(g => g.Tick(dt));
                e.PerformHook(e.updateGate, b => b.OnUpdate(dt), nameof(Update));
            }
        );
    }
    
    // No Prefab to Instantiate with SpawnController, but similarly validates definitions.
    // See if there's opportunity for centralization.
    public void Apply(EffectDefinition definition)
    {
        if (definition == null)
        {
            Debug.LogWarning($"{name} has a null EffectDefinition! Cancelling application...");
            return;
        }

        Effect effect = new(definition, this);
        // Could add `if (!effect.applyGate.IsOpen) return;`
        // But it won't actually do anything because something would have to set that gate false before this.
        Effects.Add(effect);
        effect.PerformHook(effect.applyGate, b => b.OnApply(), nameof(Apply));
    }

    public void Expire(Effect effect)
    {
        if (!Effects.Contains(effect)) return;
        effect.PerformHook(effect.expireGate, b => b.OnExpire(), nameof(Expire));
        Effects.Remove(effect);
    }
}
