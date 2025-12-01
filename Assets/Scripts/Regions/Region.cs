using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RegionSetRegistrar))]
public class Region : MonoBehaviour, IActionSource, ISpawnable
{
    public Character Owner { get; set; }
    public object Spawner { get; set; }

    public Transform Transform => transform;
    public GameObject GameObject => gameObject;

    public RegionDefinition Definition;

    readonly HashSet<GameObject> _currentTargets = new();
    Rigidbody rb;

    FloatCounter seconds;
    FloatCounter pulse;
    IntegerCounter hits;

    void Awake()
    {
        if (!TryGetComponent(out Collider collider))
            Debug.LogError($"{name} has no Collider. It was likely forgotten on the Region prefab variant.");
        else
            collider.isTrigger = true;
        
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        if (Definition.Duration > 0)
            seconds = new(Definition.Duration, 0, Definition.Duration, true, true);
        if (Definition.MaxHits > 0)
            hits = new(0, 0, Definition.MaxHits,  true, false);
        if (Definition.Period > 0)
            pulse = new(Definition.Period, 0, Definition.Period, true, true);
    }

    void Start() => ExecuteAll(Definition.OnSpawnActions);
    
    void DestroyRegion()
    {
        ExecuteAll(Definition.OnDestroyActions);
        Destroy(gameObject);
    }

    void Update()
    {
        seconds?.Decrease(Time.deltaTime);
        pulse?.Decrease(Time.deltaTime);

        if (seconds != null && seconds.Expired)
            DestroyRegion();

        if (pulse != null && pulse.Expired)
        {
            ExecuteAll(Definition.OnPulseActions);
            pulse.Reset();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!FilterTarget(other, out GameObject target) || !_currentTargets.Add(target)) return;

        Execute(Definition.OnEnterActions, target);

        hits?.Increment();
        if (hits != null && hits.Exceeded)
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
    void ExecuteAll(List<IGameAction> actions)
    {
        List<GameObject> currentTargetList = _currentTargets.ToList();
        for (int i = currentTargetList.Count - 1; i >= 0; i--)
        {
            if (currentTargetList[i] != null)
                Execute(actions, currentTargetList[i]);
        }
    }

    bool FilterTarget(Collider other, out GameObject target)
    {
        target = null;
        GameObject potentialTarget = other.gameObject;

        // Skip the owner if the region has one and if the Region is configured to skip it
        if (Owner != null
            && !Definition.AffectsSource
            && potentialTarget.TryGetComponent(out Character character)
            && character == Owner
            )
            return false;

        if ((Definition.LayerMask.value & (1 << potentialTarget.layer)) == 0)
            return false;

        target = potentialTarget;
        return target != null;
    }
}
