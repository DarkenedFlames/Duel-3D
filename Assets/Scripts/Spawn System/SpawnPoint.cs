using System;
using UnityEngine;

public abstract class SpawnPoint<T> : MonoBehaviour where T : Component
{
    [NonSerialized] public T SpawnedObject;
    public bool IsFree => SpawnedObject == null;

    public void AssignObject(GameObject spawnedGameObject)
    {
        if (!spawnedGameObject.TryGetComponent(out T component))
        {
            Debug.LogError($"{name} tried to assign object without required component {typeof(T).Name}.");
            return;
        }

        SpawnedObject = component;
        SubscribeToObjectEvents(component);
    }

    protected abstract void SubscribeToObjectEvents(T spawnedObject);
    protected abstract void UnsubscribeFromObjectEvents(T spawnedObject);
    protected virtual void OnObjectReleased()
    {
        if (SpawnedObject != null)
        {
            UnsubscribeFromObjectEvents(SpawnedObject);
            SpawnedObject = null;
        }
    }

    protected virtual void OnDestroy()
    {
        if (SpawnedObject != null)
            UnsubscribeFromObjectEvents(SpawnedObject);
    }
}
