using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pickup : MonoBehaviour
{
    [SerializeField] private float duration;
    public List<PickupInstructionBinding> bindings = new();

    private float _timer;
    private bool ZeroTime => _timer <= 0;

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
        _timer = duration;
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        TryExpire();
    }

    // Only collides with Actors and Terrain
    void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.layer != LayerMask.NameToLayer("Actors")) return;

        PerformInstruction(PickupHook.OnCollide, target);
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
        // Dont know how to fire PerformInstruction without a target
        Destroy(gameObject);
    }

    void PerformInstruction(PickupHook hook, GameObject target)
    {
        PickupContext pickupContext = new(this, target);

        foreach (PickupInstructionBinding binding in bindings)
            if (binding.Hook.Equals(hook))
                binding.Execute(pickupContext);
    }
}
