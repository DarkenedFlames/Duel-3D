using UnityEngine;
public class MLinearScale : MonoBehaviour
{
    [Header("Linear Scale Settings")]
    [Tooltip("The desired scale after the elapsed time.")]
    public Vector3 TargetScale = Vector3.one * 2f;

    [Tooltip("The rate (meters/second) at which the scale increases.")]
    public float ScaleRate = 1f;

    void Start()
    {
        if (TargetScale.x <= 0 && TargetScale.y <= 0 && TargetScale.z <= 0)
            Debug.LogError($"{name}'s Mover {nameof(MLinearScale)} was configured with an invalid parameter: {nameof(TargetScale)} must be non-zero (might be too small)!");
        if (ScaleRate <= 0)
            Debug.LogError($"{name}'s Mover {nameof(MLinearScale)} was configured with an invalid parameter: {nameof(ScaleRate)} must be positive!");
    }

    void Update() => transform.localScale = Vector3.MoveTowards(transform.localScale, TargetScale, ScaleRate * Time.deltaTime);
}
