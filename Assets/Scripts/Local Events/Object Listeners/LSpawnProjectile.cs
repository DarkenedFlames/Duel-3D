using UnityEngine;

[System.Serializable]
public class LSpawnProjectile : EventReaction
{
    [Header("SpawnProjectile Configuration")]
    [Tooltip("The prefab to spawn."), SerializeField] 
    GameObject prefab;

    [Tooltip("Local spawn offset."), SerializeField]
    Vector3 spawnOffset = Vector3.zero;

    [Tooltip("Local rotation offset."), SerializeField]
    Vector3 localEulerRotation = Vector3.zero;

    public override void OnEvent(EventContext context)
    {
        if (context is PositionContext cxt)
            if (cxt.localTransform.TryGetComponent(out IHasSourceActor hasSource))
            {
                Vector3 spawnPosition = cxt.localTransform.TransformPoint(spawnOffset);
                Quaternion spawnRotation = cxt.localTransform.rotation * Quaternion.Euler(localEulerRotation);
                SpawnerController.Instance.SpawnProjectile(prefab, spawnPosition, spawnRotation, hasSource.SourceActor);
            }
    }
}










