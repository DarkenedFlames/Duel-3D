using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(ActorTargeting))]
public class Pickup : LocalEventSource
{
    [SerializeField] float duration = 100f;
    [SerializeField] float pickupsAllowed = 1;

    float _timer;
    float _remainingPickups;

    ActorTargeting targeting;

    void Awake()
    {
        _timer = duration;
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

        if (_timer <= 0 || _remainingPickups >= pickupsAllowed)
        {
            Fire(Event.OnExpire, new NullContext());
            Destroy(gameObject);          
        }
    }

    void HandleActorEnter(GameObject actor)
    {
        _remainingPickups--;
        Fire(Event.OnCollide, new TargetContext() {target = actor});
    }
}
