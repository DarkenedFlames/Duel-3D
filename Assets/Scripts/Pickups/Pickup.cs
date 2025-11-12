using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pickup : MonoBehaviour
{
    [SerializeField] private float duration;

    [SerializeField] private List<PickupBehaviorDefinition> behaviorDefinitions;
    private List<PickupBehavior> behaviors;

    private float _timer;
    private bool ZeroTime => _timer <= 0;

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;

        behaviors = behaviorDefinitions
            .Select(b => b.CreateRuntimeBehavior(this))
            .ToList();

        _timer = duration;
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        TryExpire();
    }

    // Only collides with Actors layer, still needs to check for Player tag.
    void OnTriggerEnter(Collider other)
    {
        GameObject actor = other.transform.root.gameObject;

        if (!actor.CompareTag("Player")) return;

        behaviors.ForEach(b => b.OnCollide(actor));
        Expire();
    }

    bool TryExpire()
    {
        if (!ZeroTime) return false;
        Expire();
        return true;
    }

    void Expire()
    {
        behaviors.ForEach(b => b.OnExpire());
        Destroy(gameObject);
    }
}
