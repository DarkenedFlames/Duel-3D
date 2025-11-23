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

    public void Execute(GameObject source, GameObject target)
    {
        Vector3 spawnPosition = source.transform.TransformPoint(spawnOffset);
        Quaternion spawnRotation = source.transform.rotation * Quaternion.Euler(localEulerRotation);

        GameObject Instance = Object.Instantiate(prefab, spawnPosition, spawnRotation);

        if (Instance.TryGetComponent(out IRequiresSource newObject) && source.TryGetComponent(out IRequiresSource oldObject))
            newObject.Source = oldObject.Source;
    }
}










