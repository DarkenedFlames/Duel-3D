using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pickup : LocalEventSource
{
    [SerializeField] float duration = 100f;
    [SerializeField] float pickups = 1;

    float _timer;
    float _remainingPickups;

    ActorTargeting targeting;

    void Awake()
    {
        _timer = duration;
    }

    void Update()
    {
        _timer -= Time.deltaTime;

        foreach (GameObject actor in targeting.EnteringTargets)
            Fire(Event.OnCollide, new TargetContext() {target = actor});

        if (_timer <= 0 || _remainingPickups <= 0)
        {
            Fire(Event.OnExpire, new NullContext());
            Destroy(gameObject);          
        }
    }
}
