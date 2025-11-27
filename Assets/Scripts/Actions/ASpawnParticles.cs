using UnityEngine;
public class ASpawnParticles : IGameAction
{
    [Header("Particle Settings")]
    public GameObject prefab;

    [Tooltip("Local spawn offset."), SerializeField]
    Vector3 spawnOffset = Vector3.zero;

    [Tooltip("Local rotation offset."), SerializeField]
    Vector3 localEulerRotation = Vector3.zero;

    public void Execute(ActionContext context)
    {
        if (prefab == null)
        {
            Debug.LogError($"Action {nameof(ASpawnParticles)} was configured with a null parameter: {nameof(prefab)}!");
            return;
        }
        if (context.Source == null)
        {
            Debug.LogError($"Action {nameof(ASpawnObject)} was passed a null parameter: {nameof(context.Source)}!");
            return;
        }
        if(!context.TryGetSourceTransform(out Transform sourceTransform))
        {
            Debug.LogError($"Action {nameof(ASpawnObject)} could not find {nameof(sourceTransform)}!");
            return;
        }

        Vector3 spawnPosition = sourceTransform.TransformPoint(spawnOffset);
        Quaternion spawnRotation = sourceTransform.rotation * Quaternion.Euler(localEulerRotation);

        GameObject instance = Object.Instantiate(prefab, spawnPosition, spawnRotation);
    }
}