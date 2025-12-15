using UnityEngine;

public class PickupSpawnPoint : SpawnPoint<Region>
{
    public Region Pickup => SpawnedObject;

    protected override void SubscribeToObjectEvents(Region spawnedObject) =>
        spawnedObject.OnDestroyed += OnPickupDestroyed;

    protected override void UnsubscribeFromObjectEvents(Region spawnedObject) =>
        spawnedObject.OnDestroyed -= OnPickupDestroyed;
    

    void OnPickupDestroyed() => OnObjectReleased();
}