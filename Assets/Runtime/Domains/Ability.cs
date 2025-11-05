using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AbilityType { Primary, Secondary, Utility, Special }

public class Ability
{
    public AbilityDefinition Definition { get; private set; }
    public List<AbilityBehavior> Behaviors { get; private set; }
    public AbilityHandler Handler;

    public Gate<AbilityKey> castGate = new();
    public Gate<AbilityKey> updateGate = new();

    public Ability(AbilityDefinition definition, AbilityHandler handler)
    {
        Definition = definition;
        Handler = handler;
        Behaviors = definition.behaviors.Select(b => b.CreateRuntimeInstance(this)).ToList();
    }

    public void PerformHook(Gate<AbilityKey> gate, System.Action<AbilityBehavior> behaviorAction, string hookName)
    {
        if (!gate.IsOpen)
        {
            Debug.Log($"{Handler.gameObject.name}'s {Definition.abilityName} cannot {hookName}. Reasons: {gate.GetLockSummary()}");
            return;
        }
        Behaviors.ForEach(behaviorAction);
    }

    public List<Gate<AbilityKey>> GetAllGates() => new() { castGate, updateGate };
}

