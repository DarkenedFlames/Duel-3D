using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody), typeof(MeshRenderer))]
public class Area : MonoBehaviour
{
    [Header("Area Settings")]
    public float radius = 5f;
    public float duration = 5f;
    public LayerMask targetLayers;
    public List<AreaBehaviorDefinition> behaviorDefinitions;

    [NonSerialized] public GameObject sourceActor;
    [NonSerialized] public GameObject targetActor;

    // Runtime tracking
    readonly List<GameObject> currentTargets = new();
    readonly HashSet<GameObject> previousTargets = new();
    List<AreaBehavior> behaviors = new();
    float _timer;
    bool ZeroTime => _timer <= 0;

    public IReadOnlyList<GameObject> CurrentTargets => currentTargets;

    public void SetSource(GameObject source) => sourceActor = source;
    public void SetTarget(GameObject source) => targetActor = source;

    private void Awake()
    {
        var col = GetComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = radius;

        var mesh = GetComponent<MeshRenderer>();
        float diameter = radius * 2f;
        mesh.transform.localScale = new Vector3(diameter, diameter, diameter);

        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

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
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, targetLayers, QueryTriggerInteraction.Collide);
        currentTargets.Clear();

        foreach (var col in hits)
        {
            GameObject root = col.transform.root.gameObject;
            currentTargets.Add(root);
        }

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
