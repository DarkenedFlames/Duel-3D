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

    [Tooltip("If the selected reference is a character (Owner or Target) and this is true, the spawned object will use the character's look rotation."), SerializeField]
    bool followOwnerCamera = false;

    [Tooltip("The radius around the reference at which the object will be spawned. Only enabled if Follow Owner Camera is true."), SerializeField, Min(0)]
    float cameraModeRadius = 1f;

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

        // SINGLEPLAYER ONLY: 
        // Placeholder. Once multiplayer, we need to somehow get the camera that belongs to the specific character.
        bool cameraMode = reference == ReferenceTransform.Owner && followOwnerCamera;
        Vector3 spawnPosition = cameraMode
            ? referenceTransform.position + (referenceTransform.position - Camera.main.transform.position).normalized * cameraModeRadius
            : referenceTransform.position;

        spawnPosition += referenceTransform.TransformDirection(spawnOffset);

        Quaternion spawnRotation = cameraMode
            ? Camera.main.transform.rotation
            : referenceTransform.rotation;

        spawnRotation *= Quaternion.Euler(localEulerRotation);

        GameObject instance = Object.Instantiate(prefab, spawnPosition, spawnRotation);

        // If what we spawn is an action source, set its owner to the Spawner's owner.
        if (instance.TryGetComponent(out IActionSource newSource))
        {
            newSource.Owner = context.Source.Owner;
            newSource.Magnitude = context.Source.Magnitude;
        }
        
        // If Spawned object is ISpawnable, then set the spawned object's Spawner.
        if (instance.TryGetComponent(out ISpawnable spawned))
            spawned.Spawner = context.Source;
    }
}