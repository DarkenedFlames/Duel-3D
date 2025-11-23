using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[Flags] public enum RegionExecutionType { OnSpawn, Periodic, OnEnter, OnExit, OnDestroy }
[Flags] public enum RegionExpiryType { Duration, HitLimit }

public class Region : MonoBehaviour, IRequiresSource
{
    public RegionDefinition Definition;

    private readonly HashSet<GameObject> _currentTargets = new();
    public GameObject Source {get; set;}
    FloatCounter seconds;
    FloatCounter pulse;
    IntegerCounter hits;

    void Awake()
    {
        seconds = new(Definition.Duration, 0, Definition.Duration, true, true);
        hits    = new(0,                   0, Definition.MaxHits,  true, false);
        pulse   = new(Definition.Period,   0, Definition.Period,   true, true);
    }

    void Start()
    {
        if (Definition.RegionExecutionType.HasFlag(RegionExecutionType.OnSpawn))
            ExecuteAll();
    }

    void DestroyRegion()
    {
        if (Definition.RegionExecutionType.HasFlag(RegionExecutionType.OnDestroy))
            ExecuteAll();
        Destroy(gameObject);
    }

    void Update()
    {
        Definition.Mover?.Tick(this);

        seconds.Decrease(Time.deltaTime);
        pulse.Decrease(Time.deltaTime);

        if (Definition.RegionExpiryType.HasFlag(RegionExpiryType.Duration) && seconds.Expired)
            DestroyRegion();

        if (Definition.RegionExecutionType.HasFlag(RegionExecutionType.Periodic) && pulse.Expired)
            ExecuteAll();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!FilterTarget(other, out GameObject target)) return;
        _currentTargets.Add(target);

        hits.Increment();

        if (Definition.RegionExecutionType.HasFlag(RegionExecutionType.OnEnter))
            Execute(target);

        if (Definition.RegionExpiryType.HasFlag(RegionExpiryType.HitLimit) && hits.Exceeded)
            DestroyRegion();
    }

    void OnTriggerExit(Collider other)
    {
        if (!FilterTarget(other, out GameObject target)) return;
        _currentTargets.Remove(target);

        if (Definition.RegionExecutionType.HasFlag(RegionExecutionType.OnExit))
            Execute(target);
    }

    void Execute(GameObject target) => Definition.Actions.ForEach(a => a.Execute(gameObject, target));
    void ExecuteAll() => _currentTargets.ToList().ForEach(t => Execute(t));

    bool FilterTarget(Collider other, out GameObject target)
    {
        target = null;
        GameObject potentialTarget = other.gameObject;
        if ((Definition.LayerMask.value & (1 << potentialTarget.layer)) == 0)
            return false;

        return potentialTarget != null;
    }
}
