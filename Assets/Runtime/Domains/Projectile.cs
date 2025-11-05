using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile
{
    public ProjectileDefinition Definition { get; private set; }
    public List<ProjectileBehavior> Behaviors { get; private set; }
    public ProjectileHandler Handler { get; private set; }

    public Gate<ProjectileKey> spawnGate = new();
    public Gate<ProjectileKey> updateGate = new();
    public Gate<ProjectileKey> expireGate = new();
    public Gate<ProjectileKey> triggerGate = new();

    public Projectile(ProjectileDefinition definition, ProjectileHandler handler)
    {
        Definition = definition;
        Handler = handler;
        Behaviors = definition.behaviors.Select(b => b.CreateRuntimeInstance(this)).ToList();
    }

    public void PerformHook(Gate<ProjectileKey> gate, System.Action<ProjectileBehavior> behaviorAction, string hookName)
    {
        if (!gate.IsOpen)
        {
            Debug.Log($"{Handler.gameObject.name}'s {Definition.projectileName} cannot {hookName}. Reasons: {gate.GetLockSummary()}");
            return;
        }
        Behaviors.ForEach(behaviorAction);
    }

    public List<Gate<ProjectileKey>> GetAllGates() => new() { spawnGate, updateGate, expireGate, triggerGate };
}
