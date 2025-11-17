using UnityEngine;

[RequireComponent(typeof(Collider), typeof(NonActorController))]
public class Projectile : LocalEventSource, IHasSourceActor
{
    [SerializeField] bool collidesWithSource;
    [SerializeField] int pierces;
    [SerializeField] float duration;

    int _pierced;
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
        _pierced = pierces;
    }

    void Update()
    {
        _timer -= Time.deltaTime;

        foreach (GameObject actor in targeting.EnteringTargets)
            if (actor != SourceActor || collidesWithSource)
            {
                _pierced--;
                Fire(Event.OnCollide, new PositionContext() {target = actor, localTransform = transform});
            }

        if (_pierced <= 0 || _timer <=0)
        {
            Fire(Event.OnExpire, new NullContext());
            Destroy(gameObject);
        }
    }
}
