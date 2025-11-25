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

    public void Execute(GameObject source, GameObject _)
    {
        Vector3 spawnPosition = source.transform.TransformPoint(spawnOffset);
        Quaternion spawnRotation = source.transform.rotation * Quaternion.Euler(localEulerRotation);

        GameObject instance = Object.Instantiate(prefab, spawnPosition, spawnRotation);

        if (!instance.TryGetComponent(out SpawnContext instanceContext))
        {
            Debug.LogError($"Trying to spawn {instance.name} but it has no SpawnContext");
            return;
        }

        instanceContext.Spawner = source; // Remains null if not spawned by something

        if(!source.TryGetComponent(out SpawnContext sourceContext))
        {
            Debug.Log($"Spawner {source.name} had no spawn context. Defaulting Owner of {instance.name} to Spawner {source.name}.");
            instanceContext.Owner = source;
        }
        else
            instanceContext.Owner = sourceContext.Owner; // Only is null if the source was not spawned by something
    }
}