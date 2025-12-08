using UnityEngine;
public class ASpawnParticlesTargeted : ITargetedAction
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
            LogFormatter.LogNullField(nameof(prefab), nameof(ASpawnParticlesTargeted), context.Source.GameObject);
            return;
        }
        if (context.Target == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Target), nameof(Execute), nameof(ASpawnParticlesTargeted), context.Source.GameObject);
            return;
        }

        Vector3 spawnPosition = context.Target.transform.TransformPoint(spawnOffset);
        Quaternion spawnRotation = context.Target.transform.rotation * Quaternion.Euler(localEulerRotation);

        GameObject instance = Object.Instantiate(prefab, spawnPosition, spawnRotation);
    }
}