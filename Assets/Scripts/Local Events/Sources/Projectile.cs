using UnityEngine;

[RequireComponent(typeof(Collider), typeof(NonActorController))]
[RequireComponent(typeof(ActorTargeting))]
public class Projectile : LocalEventSource, IHasSourceActor
{
    [SerializeField] bool collidesWithSource = false;
    [SerializeField] int hitsAllowed = 1;
    [SerializeField] float duration = 10f;

    int _hits;
    float _timer;

    ActorTargeting targeting;

    public GameObject SourceActor { get; set; }

    public void SetSource(GameObject source)
    {
        if (source.layer == LayerMask.NameToLayer("Actors"))
            SourceActor = source;
    }

    void Awake()
    {
        _timer = duration;
        _hits = 0;
        Debug.Log($"{_hits} / {hitsAllowed} hits spent.");

        targeting = GetComponent<ActorTargeting>();
    }

    void OnEnable()
    {
        targeting.OnActorEnter += HandleActorEnter;
    }

    void OnDisable()
    {
        targeting.OnActorEnter -= HandleActorEnter;
    }

    void Update()
    {
        _timer -= Time.deltaTime;

        if (_hits >= hitsAllowed || _timer <= 0)
        {
            Fire(Event.OnExpire, new NullContext());
            Destroy(gameObject);
        }
    }

    void HandleActorEnter(GameObject actor)
    {
        if (actor != SourceActor || collidesWithSource)
        {
            _hits++;
            Fire(Event.OnCollide, new PositionContext() {target = actor, localTransform = transform});
        }
    }
}
