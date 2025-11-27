using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[Flags] public enum RegionExpiryType { Duration, HitLimit }

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(SpawnContext))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RegionSetRegistrar))]
public class Region : MonoBehaviour
{
    public RegionDefinition Definition;

    readonly HashSet<GameObject> _currentTargets = new();
    Collider col;
    SpawnContext spawnContext;
    Rigidbody rb;

    FloatCounter seconds;
    FloatCounter pulse;
    IntegerCounter hits;

    void Awake()
    {
        spawnContext = GetComponent<SpawnContext>();

        col = GetComponent<Collider>();
        col.isTrigger = true;
        
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        seconds = new(Definition.Duration, 0, Definition.Duration, true, true);
        hits    = new(0,                   0, Definition.MaxHits,  true, false);
        pulse   = new(Definition.Period,   0, Definition.Period,   true, true);
    }

    void Start() => ExecuteAll(Definition.OnSpawnActions);
    
    void DestroyRegion()
    {
        ExecuteAll(Definition.OnDestroyActions);
        Destroy(gameObject);
    }

    void Update()
    {
        seconds.Decrease(Time.deltaTime);
        pulse.Decrease(Time.deltaTime);

        if (Definition.RegionExpiryType.HasFlag(RegionExpiryType.Duration) && seconds.Expired)
            DestroyRegion();

        if (pulse.Expired)
            ExecuteAll(Definition.OnPulseActions);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!FilterTarget(other, out GameObject target) || !_currentTargets.Add(target)) return;

        Execute(Definition.OnEnterActions, target);

        hits.Increment();
        if (Definition.RegionExpiryType.HasFlag(RegionExpiryType.HitLimit) && hits.Exceeded)
            DestroyRegion();
    }

    void OnTriggerExit(Collider other)
    {
        if (!FilterTarget(other, out GameObject target) || !_currentTargets.Remove(target)) return;

        Execute(Definition.OnExitActions, target);
    }

    void Execute(List<IGameAction> actions, GameObject target)
    {
        if (!target.TryGetComponent(out Character character)) return;
        
        ActionContext context = new(){ Source = this, Target = character };
        actions.ForEach(a => a.Execute(context));
    }
    void ExecuteAll(List<IGameAction> actions) => _currentTargets.ToList().ForEach(t => Execute(actions, t));

    bool FilterTarget(Collider other, out GameObject target)
    {
        target = null;
        GameObject potentialTarget = other.gameObject;

        if (spawnContext.Owner != null 
            && !Definition.AffectsSource
            && potentialTarget.TryGetComponent(out Character character)
            && character == spawnContext.Owner
            )
            return false;
        if ((Definition.LayerMask.value & (1 << potentialTarget.layer)) == 0)
            return false;

        target = potentialTarget;
        return target != null;
    }
}
