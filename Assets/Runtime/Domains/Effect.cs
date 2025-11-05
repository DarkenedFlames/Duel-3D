using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Effect
{
    public EffectDefinition Definition { get; private set; }
    public List<EffectBehavior> Behaviors { get; private set; }
    public EffectHandler Handler;

    public Gate<EffectKey> applyGate = new();
    public Gate<EffectKey> updateGate = new();
    public Gate<EffectKey> expireGate = new();

    public Effect(EffectDefinition definition, EffectHandler handler)
    {
        Definition = definition;
        Handler = handler;
        Behaviors = definition.behaviors.Select(b => b.CreateRuntimeInstance(this)).ToList();
    }

    public void PerformHook(Gate<EffectKey> gate, System.Action<EffectBehavior> behaviorAction, string hookName)
    {
        if (!gate.IsOpen)
        {
            Debug.Log($"{Handler.gameObject.name}'s {Definition.effectName} cannot {hookName}. Reasons: {gate.GetLockSummary()}");
            return;
        }
        Behaviors.ForEach(behaviorAction);
    }

    public List<Gate<EffectKey>> GetAllGates() => new() { applyGate, updateGate, expireGate };
}