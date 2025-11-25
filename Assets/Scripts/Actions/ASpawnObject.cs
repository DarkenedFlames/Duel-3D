using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class ASpawnObject : IGameAction
{
    [Header("SpawnArea Configuration")]
    [Tooltip("The object prefab to spawn."), SerializeField] 
    GameObject prefab;

    [Tooltip("Local spawn offset."), SerializeField]
    Vector3 spawnOffset = Vector3.zero;

    [Tooltip("Local rotation offset."), SerializeField]
    Vector3 localEulerRotation = Vector3.zero;

    public void Execute(GameObject source, GameObject _)
    {
        Vector3 spawnPosition = source.transform.TransformPoint(spawnOffset);
        Quaternion spawnRotation = source.transform.rotation * Quaternion.Euler(localEulerRotation);


        GameObject instance = Object.Instantiate(prefab, spawnPosition, spawnRotation);

        SpawnContext sourceContext = source.GetComponent<SpawnContext>();

        if (!instance.TryGetComponent(out SpawnContext instanceContext))
            Debug.LogError($"Trying to spawn {instance.name} but it has no SpawnContext");

        instanceContext.Spawner = source;
        instanceContext.Owner = sourceContext != null ? sourceContext.Owner : source;
    }
}