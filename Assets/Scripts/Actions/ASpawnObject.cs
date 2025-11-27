using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ASpawnObject : IGameAction
{
    [Header("SpawnArea Configuration")]
    [Tooltip("The object prefab to spawn."), SerializeField] 
    GameObject prefab; // Must have a SpawnContext Component

    [Tooltip("Local spawn offset."), SerializeField]
    Vector3 spawnOffset = Vector3.zero;

    [Tooltip("Local rotation offset."), SerializeField]
    Vector3 localEulerRotation = Vector3.zero;

    public void Execute(ActionContext context)
    {
        if (prefab == null)
        {
            Debug.LogError($"Action {nameof(ASpawnObject)} was configured with a null parameter: {nameof(prefab)}!");
            return;
        }
        if (context.Source == null)
        {
            Debug.LogError($"Action {nameof(ASpawnObject)} was passed a null parameter: {nameof(context.Source)}!");
            return;
        }
        if(!context.TryGetSourceTransform(out Transform sourceTransform))
        {
            Debug.LogError($"Action {nameof(ASpawnObject)} could not find {nameof(sourceTransform)}!");
            return;
        }

        Vector3 spawnPosition = sourceTransform.TransformPoint(spawnOffset);
        Quaternion spawnRotation = sourceTransform.rotation * Quaternion.Euler(localEulerRotation);

        GameObject instance = Object.Instantiate(prefab, spawnPosition, spawnRotation);
        if (!instance.TryGetComponent(out SpawnContext instanceContext))
        {
            Debug.LogError($"Action {nameof(ASpawnObject)} instantiated a GameObject with a missing component: {instance.name} missing {nameof(SpawnContext)}! Destroying {instance.name}...");
            Object.Destroy(instance);
            return;
        }

        instanceContext.Initialize(context.Source);
    }
}