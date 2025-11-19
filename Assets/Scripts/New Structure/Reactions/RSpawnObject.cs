using UnityEngine;

public class RSpawnObject : Reaction
{
    [Header("SpawnArea Configuration")]
    [Tooltip("The object prefab to spawn."), SerializeField] 
    GameObject prefab;

    [Tooltip("Local spawn offset."), SerializeField]
    Vector3 spawnOffset = Vector3.zero;

    [Tooltip("Local rotation offset."), SerializeField]
    Vector3 localEulerRotation = Vector3.zero;

    public void SpawnObject()
    {
        GameObject source = null;
        if (TryGetComponent(out RequiresSource requiresSource))
            source = requiresSource.Source;

        Vector3 spawnPosition = transform.TransformPoint(spawnOffset);
        Quaternion spawnRotation = transform.rotation * Quaternion.Euler(localEulerRotation);
        SpawnerController.Instance.SpawnArea(prefab, spawnPosition, spawnRotation, source); // THIS CAN BE NULL NOW, WATCH OUT
    }
}










