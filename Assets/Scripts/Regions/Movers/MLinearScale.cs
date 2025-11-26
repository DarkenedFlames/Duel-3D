using UnityEngine;
public class MLinearScale : MonoBehaviour
{
    [Header("Linear Scale Settings")]
    [Tooltip("The desired scale after the elapsed time.")]
    public Vector3 TargetScale = Vector3.one * 2f;

    [Tooltip("The rate (meters/second) at which the scale increases.")]
    public float ScaleRate = 1f;

    void Update() => transform.localScale = Vector3.MoveTowards(transform.localScale, TargetScale, ScaleRate * Time.deltaTime);
}
