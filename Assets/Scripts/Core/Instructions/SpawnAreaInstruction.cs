
using System;
using UnityEngine;

[Serializable]
public class SpawnAreaInstruction : IInstruction
{
    [Tooltip("The prefab to spawn.")]
    public GameObject prefab;

    [Tooltip("Local spawn offset.")]
    public Vector3 spawnOffset = Vector3.zero;

    [Tooltip("Local rotation offset.")]
    public Vector3 localEulerRotation = Vector3.zero;

    public void Execute(IInstructionContext context)
    {
        if (context.Domain.TryGetComponent(out IHasSourceActor hasSource))
        {
            Transform domainTransform = context.Domain.transform;
            Vector3 spawnPosition = domainTransform.TransformPoint(spawnOffset);
            Quaternion spawnRotation = domainTransform.rotation * Quaternion.Euler(localEulerRotation);
            SpawnerController.Instance.SpawnArea(prefab, spawnPosition, spawnRotation, hasSource.SourceActor);
            // Have to make this able to spawn anything
        }
    }
}










