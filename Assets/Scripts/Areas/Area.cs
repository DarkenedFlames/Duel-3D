using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody), typeof(MeshRenderer))]
public class Area : MonoBehaviour
{
    [Header("Area Settings")]
    public List<AreaBehaviorDefinition> behaviorDefinitions;
    public GameObject sourceActor;
    public GameObject targetActor;
    public float radius = 5f;
    public float duration = 5f;

    // Runtime tracking
    readonly List<GameObject> currentTargets = new();
    readonly HashSet<GameObject> previousTargets = new();
    readonly List<AreaBehavior> behaviors = new();
    float lifetime;
    bool expired;

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

        foreach (var def in behaviorDefinitions)
        {
            if (def != null)
                behaviors.Add(def.CreateRuntimeBehavior(this));
        }

        lifetime = duration;
    }

    void Start() => behaviors.ForEach(b => b.OnStart());
    
    void Update()
    {
        float dt = Time.deltaTime;
        lifetime -= dt;

        UpdateCurrentTargets();

        foreach (var behavior in behaviors) behavior.OnTick(dt);

        if (lifetime <= 0f && !expired) Expire();
    }

    void UpdateCurrentTargets()
    {
        // Rebuild current
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        currentTargets.Clear();

        foreach (var col in hits)
        {
            GameObject root = col.transform.root.gameObject;
            if (!root.CompareTag("Actor")) continue;

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

        // Set current equal to previous
        previousTargets.Clear();
        foreach (var actor in currentTargets)
            previousTargets.Add(actor);
    }

    void Expire()
    {
        expired = true;
        behaviors.ForEach(b => b.OnExpire());
        Destroy(gameObject);
    }
}
