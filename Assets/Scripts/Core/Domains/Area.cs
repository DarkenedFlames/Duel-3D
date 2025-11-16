using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(SphereCollider), typeof(MeshRenderer), typeof(NonActorController))]
public class Area : MonoBehaviour, IHasSourceActor
{
    [Header("Area Settings")]
    public float radius = 5f;
    public float duration = 5f;
    public float period = 1f;
    public bool affectsSource = false;

    public List<AreaInstructionBinding> bindings = new();

    // Runtime tracking
    private HashSet<GameObject> currentTargets = new();
    private HashSet<GameObject> previousTargets = new();
    float _timer;
    float _pulseTimer;
    bool ZeroTime => _timer <= 0;

    NonActorController controller;

    public GameObject SourceActor { get; set; }
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

        _timer = duration;
        _pulseTimer = period;
    }

    
    void Update()
    {
        UpdateCurrentTargets();

        float dt = Time.deltaTime;

        _pulseTimer -= dt;
        TryPulse();

        _timer -= dt;
        TryExpire();
    }

    // Collides with actors and terrain
    void UpdateCurrentTargets()
    {
        var targetLayers = LayerMask.GetMask("Actors");
        
        // Rebuild current
        currentTargets.Clear();
        currentTargets = Physics.OverlapSphere(transform.position, radius, targetLayers, QueryTriggerInteraction.Collide)
            .Select(col => col.gameObject)
            .ToHashSet();

        if (currentTargets.Contains(SourceActor) && !affectsSource)
            currentTargets.Remove(SourceActor);

        // Entry: in current but not in previous
        foreach (var actor in currentTargets)
            if (!previousTargets.Contains(actor))
                PerformInstruction(AreaHook.OnTargetEnter, actor);
            
        // Exit: in previous but not in current
        foreach (var actor in previousTargets)
            if (!currentTargets.Contains(actor))
                PerformInstruction(AreaHook.OnTargetExit, actor);

        // Set previous equal to current
        previousTargets.Clear();
        foreach (var actor in currentTargets)
            previousTargets.Add(actor);
    }

    bool TryExpire()
    {
        if (!ZeroTime) return false;

        foreach (GameObject actor in currentTargets)
            PerformInstruction(AreaHook.OnExpire, actor);

        Destroy(gameObject);
        return true;
    }
    
    bool TryPulse()
    {
        if (_pulseTimer >= period) return false;

        _pulseTimer = period;
        foreach (GameObject actor in currentTargets)
            PerformInstruction(AreaHook.OnPulse, actor);

        return true;
    }

    void PerformInstruction(AreaHook hook, GameObject target)
    {
        AreaContext areaContext = new(this, target);

        foreach (AreaInstructionBinding binding in bindings)
            if (binding.Hook.Equals(hook))
                binding.Execute(areaContext);
    }
}
