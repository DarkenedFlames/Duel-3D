using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RegionSetRegistrar))]
public class Region : MonoBehaviour, IActionSource, ISpawnable, IDespawnable
{
    public Character Owner { get; set; }
    public object Spawner { get; set; }
    public float Magnitude { get; set; } = 1f;

    public Transform Transform => transform;
    public GameObject GameObject => gameObject;

    public RegionDefinition Definition;

    readonly Dictionary<GameObject, int> _targetColliderCounts = new();

    Rigidbody rb;

    FloatCounter seconds;
    FloatCounter pulse;
    IntegerCounter hits;

    bool spawned = false;
    public event Action OnDestroyed;
    public event Action<GameObject> OnDespawned;

    void Awake()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        if (colliders.Length == 0)
            Debug.LogError($"{name} has no Colliders. They were likely forgotten on the Region prefab variant.");
        else
        {
            foreach (Collider col in colliders)
                col.isTrigger = true;
        }
        
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        if (Definition.Duration > 0)
            seconds = new(Definition.Duration, 0, Definition.Duration, true, true);
        if (Definition.MaxHits > 0)
            hits = new(0, 0, Definition.MaxHits, true, false);
        if (Definition.Period > 0)
            pulse = new(Definition.Period, 0, Definition.Period, true, true);
    }

    void DestroyRegion()
    {
        ExecuteAllTargeted(RegionHook.OnDestroyPerTarget);
        Execute(RegionHook.OnDestroy);
        OnDespawned?.Invoke(gameObject);
        OnDestroyed?.Invoke();
        Destroy(gameObject);
    }

    void Update()
    {
        if (!spawned)
        {
            Execute(RegionHook.OnSpawn);
            ExecuteAllTargeted(RegionHook.OnSpawnPerTarget);
            spawned = true;
            return;
        }

        seconds?.Decrease(Time.deltaTime);
        pulse?.Decrease(Time.deltaTime);

        if (seconds != null && seconds.Expired)
        {
            DestroyRegion();
            return;
        }

        if (pulse != null && pulse.Expired)
        {
            Execute(RegionHook.OnPulse);
            ExecuteAllTargeted(RegionHook.OnPulsePerTarget);
            pulse.Reset();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!FilterTarget(other, out GameObject target)) return;

        if (!_targetColliderCounts.ContainsKey(target))
        {
            _targetColliderCounts[target] = 1;
            Execute(RegionHook.OnTargetEnter, target);

            hits?.Increment();
            if (hits == null || !hits.Exceeded) return;
            
            DestroyRegion();
            return;
        }
        else
            _targetColliderCounts[target]++;
    }

    void OnTriggerExit(Collider other)
    {
        if (!FilterTarget(other, out GameObject target)) return;
        if (!_targetColliderCounts.ContainsKey(target)) return;
        if (--_targetColliderCounts[target] > 0) return;
        
        _targetColliderCounts.Remove(target);
        Execute(RegionHook.OnTargetExit, target);
        
    }

    void ExecuteAllTargeted(RegionHook hook)
    {
        List<GameObject> currentTargetList = _targetColliderCounts.Keys.ToList();
        for (int i = currentTargetList.Count - 1; i >= 0; i--)
        {
            GameObject target = currentTargetList[i];
            if (target != null)
                Execute(hook, target);
        }
    }

    void Execute(RegionHook hook, GameObject target)
    {
        if (target.TryGetComponent(out Character character))
            Definition.ExecuteActions(hook, new() { Source = this, Target = character, Magnitude = Magnitude });
    }

    void Execute(RegionHook hook) => Definition.ExecuteActions(hook, new() { Source = this, Target = null, Magnitude = Magnitude });
    

    bool FilterTarget(Collider other, out GameObject target)
    {
        target = null;
        GameObject potentialTarget = other.gameObject;

        // Skip the owner if the region has one and if the Region is configured to skip it
        if (Owner != null
            && !Definition.AffectsSource
            && potentialTarget.TryGetComponent(out Character character)
            && character == Owner)
            return false;

        if ((Definition.LayerMask.value & (1 << potentialTarget.layer)) == 0)
            return false;

        target = potentialTarget;
        return target != null;
    }
}
