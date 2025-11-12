using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon
{
    public WeaponDefinition Definition { get; private set; }
    public List<WeaponBehavior> Behaviors { get; private set; }
    public WeaponHandler Handler { get; private set; }

    public Gate<WeaponKey> equipGate = new();
    public Gate<WeaponKey> attackGate = new();
    public Gate<WeaponKey> triggerGate = new();
    public Gate<WeaponKey> updateGate = new();

    public Weapon(WeaponDefinition definition, WeaponHandler handler)
    {
        Definition = definition;
        Handler = handler;
        Behaviors = definition.behaviors.Select(b => b.CreateRuntimeInstance(this)).ToList();
    }

    public void PerformHook(Gate<WeaponKey> gate, System.Action<WeaponBehavior> behaviorAction, string hookName)
    {
        if (!gate.IsOpen)
        {
            Debug.Log($"{Handler.gameObject.name}'s {Definition.weaponName} cannot {hookName}. Reasons: {gate.GetLockSummary()}");
            return;
        }
        Behaviors.ForEach(behaviorAction);
    }

    public List<Gate<WeaponKey>> GetAllGates() => new() { equipGate, attackGate, triggerGate, updateGate };
}
