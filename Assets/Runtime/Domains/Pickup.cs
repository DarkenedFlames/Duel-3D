using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pickup
{
    public PickupDefinition Definition { get; private set; }
    public List<PickupBehavior> Behaviors { get; private set; }
    public PickupHandler Handler { get; private set; }

    public Gate<PickupKey> triggerGate = new();
    public Gate<PickupKey> spawnGate = new();
    public Gate<PickupKey> expireGate = new();
    public Gate<PickupKey> updateGate = new();

    public Pickup(PickupDefinition definition, PickupHandler handler)
    {
        Definition = definition;
        Handler = handler;
        Behaviors = definition.behaviors.Select(b => b.CreateRuntimeInstance(this)).ToList();
    }

    public void PerformHook(Gate<PickupKey> gate, System.Action<PickupBehavior> behaviorAction, string hookName)
    {
        if (!gate.IsOpen)
        {
            Debug.Log($"{Handler.gameObject.name}'s {Definition.pickupName} cannot {hookName}. Reasons: {gate.GetLockSummary()}");
            return;
        }
        Behaviors.ForEach(behaviorAction);
    }

    public List<Gate<PickupKey>> GetAllGates() => new() { triggerGate, spawnGate, expireGate, updateGate };
}
