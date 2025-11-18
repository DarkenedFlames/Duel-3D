using UnityEngine;

[System.Serializable]
public class LSpawnArea : EventReaction
{
    [Header("SpawnArea Configuration")]
    [Tooltip("The prefab to spawn."), SerializeField] 
    GameObject prefab;

    [Tooltip("Local spawn offset."), SerializeField]
    Vector3 spawnOffset = Vector3.zero;

    [Tooltip("Local rotation offset."), SerializeField]
    Vector3 localEulerRotation = Vector3.zero;

    public override void OnEvent(EventContext context)
    {
        if (context.source == null) return;
        if (context.attacker == null) return;

        Vector3 spawnPosition = context.source.transform.TransformPoint(spawnOffset);
        Quaternion spawnRotation = context.source.transform.rotation * Quaternion.Euler(localEulerRotation);
        SpawnerController.Instance.SpawnArea(prefab, spawnPosition, spawnRotation, context.attacker);
    }
}










