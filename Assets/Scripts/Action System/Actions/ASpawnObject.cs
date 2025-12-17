using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ASpawnObject : IGameAction
{

    public enum ReferenceTransform
    {
        Owner,
        Source,
        Target
    }

    [Header("Conditions")]
    [SerializeReference]
    public List<IActionCondition> Conditions;

    [Header("Action Configuration")]
    [Tooltip("The object prefab to spawn."), SerializeField] 
    GameObject prefab;

    [Tooltip("Transform to spawn at an offset from."), SerializeField]
    ReferenceTransform reference = ReferenceTransform.Source;

    [Tooltip("Local spawn offset."), SerializeField]
    Vector3 spawnOffset = Vector3.zero;

    [Tooltip("Local rotation offset."), SerializeField]
    Vector3 localEulerRotation = Vector3.zero;

    public void Execute(ActionContext context)
    {
        if (prefab == null)
        {
            LogFormatter.LogNullField(nameof(prefab), nameof(ASpawnObject), context.Source.GameObject);
            return;
        }

        Transform referenceTransform = reference switch
        {
            ReferenceTransform.Owner  => context.Source.Owner.transform,
            ReferenceTransform.Target => context.Target.transform,
            ReferenceTransform.Source => context.Source.Transform,
            _ => null
        };

        if (referenceTransform == null)
        {
            LogFormatter.LogNullArgument(nameof(referenceTransform), nameof(Execute), nameof(ASpawnObject), context.Source.GameObject);
            return;
        }

        if (Conditions != null)
        {
            foreach (IActionCondition condition in Conditions)
                if (!condition.IsSatisfied(context))
                    return;
        }

        Vector3 spawnPosition = referenceTransform.TransformPoint(spawnOffset);
        Quaternion spawnRotation = referenceTransform.rotation * Quaternion.Euler(localEulerRotation);

        GameObject instance = Object.Instantiate(prefab, spawnPosition, spawnRotation);

        // If what we spawn is an action source, set its owner to the Spawner's owner.
        if (instance.TryGetComponent(out IActionSource newSource))
            newSource.Owner = context.Source.Owner;
        
        // If Spawned object is ISpawnable, then set the spawned object's Spawner.
        if (instance.TryGetComponent(out ISpawnable spawned))
            spawned.Spawner = context.Source;
    }
}