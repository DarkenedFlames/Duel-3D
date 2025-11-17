using UnityEngine;

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
        mesh.transform.localScale = 2f * radius * Vector3.one;

        targeting = GetComponent<ActorTargeting>();

        _timer = duration;
        _pulseTimer = period;
    }

    void OnEnable()
    {
        targeting.OnActorEnter += HandleActorEnter;
        targeting.OnActorStay += HandleActorStay;
        targeting.OnActorExit += HandleActorExit;
    }

    void OnDisable()
    {
        targeting.OnActorEnter -= HandleActorEnter;
        targeting.OnActorStay -= HandleActorStay;
        targeting.OnActorExit -= HandleActorExit;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        _pulseTimer -= dt;
        if (_pulseTimer <= 0)
            _pulseTimer = period;
        
        _timer -= dt;
        if(_timer <= 0)
            Destroy(gameObject);
    }

    void InvokeGenericEvent(GameObject actor, Event evt)
    {
        if (actor != SourceActor || affectsSource)
            Fire(evt, new PositionContext() {target = actor, localTransform = transform});
    }

    void HandleActorEnter(GameObject actor) => InvokeGenericEvent(actor, Event.OnTargetEnter);

    void HandleActorExit(GameObject actor) => InvokeGenericEvent(actor, Event.OnTargetExit);
    
    void HandleActorStay(GameObject actor)
    {
        if (_pulseTimer <= 0) InvokeGenericEvent(actor, Event.OnPulse);
        if (_timer <= 0) InvokeGenericEvent(actor, Event.OnExpire);
    }

}
