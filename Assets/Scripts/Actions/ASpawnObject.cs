using UnityEngine;

[System.Serializable]
public class ASpawnObject : IGameAction
{
    [Header("SpawnArea Configuration")]
    [Tooltip("The object prefab to spawn."), SerializeField] 
    GameObject prefab; // Must implement ISpawnable, might be an IActionSource

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
        if (context.Source == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Source), nameof(Execute), nameof(ASpawnObject), context.Source.GameObject);
            return;
        }

        Vector3 spawnPosition = context.Source.Transform.TransformPoint(spawnOffset);
        Quaternion spawnRotation = context.Source.Transform.rotation * Quaternion.Euler(localEulerRotation);

        GameObject instance = Object.Instantiate(prefab, spawnPosition, spawnRotation);

        // If what we spawn is an action source, set its owner to the Spawner's owner.
        if (instance.TryGetComponent(out IActionSource newSource))
        {
            newSource.Owner = context.Source.Owner;
        }

        // Spawned object must be ISpawnable, then set the spawned object's Spawner.
        if (!instance.TryGetComponent(out ISpawnable spawned))
            Debug.LogError($"{instance.name} tried to spawn with no ISpawnable.");
        else
            spawned.Spawner = context.Source;
    }
}