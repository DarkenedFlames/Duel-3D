using UnityEngine;

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

    public void Execute(GameObject target) // This is nice... shows directly passing on the source.
    {
        /* We need a way to access the object
        GameObject source = null;
        if (TryGetComponent(out RequiresSource requiresSource))
            source = requiresSource.Source;

        Vector3 spawnPosition = transform.TransformPoint(spawnOffset);
        Quaternion spawnRotation = transform.rotation * Quaternion.Euler(localEulerRotation);
        var Instance = SpawnerController.Instance.SpawnArea(prefab, spawnPosition, spawnRotation);

        if (Instance.TryGetComponent(out RequiresSource requiresSource))
            Instance.SetSource(source);
        */
    }
}










