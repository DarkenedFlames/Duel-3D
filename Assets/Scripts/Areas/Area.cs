using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(SphereCollider), typeof(MeshRenderer), typeof(NonActorController))]
public class Area : MonoBehaviour
{
    [Header("Area Settings")]
    public float radius = 5f;
    public float duration = 5f;
    public LayerMask targetLayers;
    public List<AreaBehaviorDefinition> behaviorDefinitions;
    List<AreaBehavior> behaviors = new();

    // Runtime tracking
    private HashSet<GameObject> currentTargets = new();
    private HashSet<GameObject> previousTargets = new();
    public IReadOnlyList<GameObject> CurrentTargets => currentTargets.ToList();
    float _timer;
    bool ZeroTime => _timer <= 0;

    NonActorController controller;

    public GameObject SourceActor { get; private set; }
    public GameObject TargetActor => controller.HomingTarget;
    public void SetSource(GameObject source) => SourceActor = source;

    private void Awake()
    {
        var col = GetComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = radius;

        var mesh = GetComponent<MeshRenderer>();
        float diameter = radius * 2f;
        mesh.transform.localScale = new Vector3(diameter, diameter, diameter);

        controller = GetComponent<NonActorController>();

        behaviors = behaviorDefinitions
            .Select(b => b.CreateRuntimeBehavior(this))
            .ToList();
        
        _timer = duration;
    }

    void Start() => behaviors.ForEach(b => b.OnStart());
    
    void Update()
    {
        float dt = Time.deltaTime;
        _timer -= dt;

        UpdateCurrentTargets();
        behaviors.ForEach(b => b.OnTick(dt));
        TryExpire();
    }

    void UpdateCurrentTargets()
    {
        // Rebuild current
        currentTargets.Clear();
        currentTargets = Physics.OverlapSphere(transform.position, radius, targetLayers, QueryTriggerInteraction.Collide)
            .Select(col => col.transform.root.gameObject)
            .ToHashSet();

        // Entry: in current but not in previous
        foreach (var actor in currentTargets)
            if (!previousTargets.Contains(actor))
                behaviors.ForEach(b => b.OnTargetEnter(actor));
        
        // Exit: in previous but not in current
        foreach (var actor in previousTargets)
            if (!currentTargets.Contains(actor))
                behaviors.ForEach(b => b.OnTargetExit(actor));

        // Set previous equal to current
        previousTargets.Clear();
        foreach (var actor in currentTargets)
            previousTargets.Add(actor);
    }

    bool TryExpire()
    {
        if (!ZeroTime) return false;

        behaviors.ForEach(b => b.OnExpire());
        Destroy(gameObject);
        return true;
    }
}
