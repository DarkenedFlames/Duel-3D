using System;
using UnityEngine;

public class PickupSpawnPoint : MonoBehaviour
{
    [NonSerialized] public Region Pickup;
    public bool IsFree => Pickup == null;

    public void AssignPickup(GameObject pickup)
    {
        if (!pickup.TryGetComponent(out Region region)) return;

        Pickup = region;
        Pickup.OnDestroyed += OnPickupDestroyed;
    }

    void OnDestroy()
    {
        if (Pickup != null) Pickup.OnDestroyed -= OnPickupDestroyed;
    }
    
    void OnPickupDestroyed()
    {
        Pickup.OnDestroyed -= OnPickupDestroyed;
        Pickup = null;
    }
}