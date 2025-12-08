using UnityEngine;
public class ASpawnParticles : ISourceAction
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
            LogFormatter.LogNullField(nameof(prefab), nameof(ASpawnParticles), context.Source.GameObject);
            return;
        }
        if (context.Source == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Source), nameof(Execute), nameof(ASpawnParticles), context.Source.GameObject);
            return;
        }

        Vector3 spawnPosition = context.Source.Transform.TransformPoint(spawnOffset);
        Quaternion spawnRotation = context.Source.Transform.rotation * Quaternion.Euler(localEulerRotation);

        GameObject instance = Object.Instantiate(prefab, spawnPosition, spawnRotation);
    }
}