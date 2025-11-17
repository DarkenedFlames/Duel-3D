using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer), typeof(NonActorController))]
[RequireComponent(typeof(ActorTargeting))]
public class Area : LocalEventSource, IHasSourceActor
{
    [Header("Area Settings")]
    [SerializeField] float radius = 5f;
    [SerializeField] float duration = 5f;
    [SerializeField] float period = 1f;
    [SerializeField] bool affectsSource = false;

    float _timer;
    float _pulseTimer;

    public GameObject SourceActor { get; set; }
    public void SetSource(GameObject source) => SourceActor = source;

    ActorTargeting targeting;

    private void Awake()
    {
        var col = GetComponent<SphereCollider>();
        col.radius = radius;

        var mesh = GetComponent<MeshRenderer>();
        float diameter = radius * 2f;
        mesh.transform.localScale = new Vector3(diameter, diameter, diameter);

        targeting = GetComponent<ActorTargeting>();

        _timer = duration;
        _pulseTimer = period;
    }
    
    void Update()
    {
        float dt = Time.deltaTime;
        _timer -= dt;
        _pulseTimer -= dt;

        SignalGroupEvent(targeting.EnteringTargets, Event.OnTargetEnter);
        SignalGroupEvent(targeting.ExitingTargets, Event.OnTargetExit);

        if (_pulseTimer <= 0)
        {
            _pulseTimer = period;
            SignalGroupEvent(targeting.CurrentTargets, Event.OnPulse);
        }

        if(_timer <= 0)
        {
            SignalGroupEvent(targeting.CurrentTargets, Event.OnExpire);
            Destroy(gameObject);
        }
    }

    void SignalGroupEvent(IEnumerable<GameObject> actors, Event evt)
    {
        foreach (GameObject actor in actors)
            if (actor != SourceActor || affectsSource)
               Fire(evt, new PositionContext() {target = actor, localTransform = transform});
    }
}
