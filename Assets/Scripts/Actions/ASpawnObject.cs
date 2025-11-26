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
        if (!context.TryGetSourceTransform(out Transform sourceTransform))
        {
            Debug.LogError("Could not find source transform in ASpawnObject");
            return;
        }

        
        // ADD NULL CHECKS

        Vector3 spawnPosition = sourceTransform.TransformPoint(spawnOffset);
        Quaternion spawnRotation = sourceTransform.rotation * Quaternion.Euler(localEulerRotation);

        GameObject instance = Object.Instantiate(prefab, spawnPosition, spawnRotation);

        if (!instance.TryGetComponent(out SpawnContext instanceContext))
        {
            Debug.LogError($"Trying to spawn {instance.name} but it has no SpawnContext");
            return;
        }

        instanceContext.Initialize(context.Source);
    }
}