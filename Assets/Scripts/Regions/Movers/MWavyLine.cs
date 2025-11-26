using UnityEngine;
public class MWavyLine : MonoBehaviour
{
    [Header("Wavy Line Settings")]
    [Tooltip("The speed (meters/second) at which the object moves foward.")]
    public float ForwardSpeed = 8f;

    [Tooltip("The range (meters) at which the object strafes.")]
    public float Amplitude = 1f;

    [Tooltip("The frequency (Hz) at which the wavy path repeats.")]
    public float Frequency = 2f;

    [Tooltip("The offset (radians) within the path at which the object starts.")]
    public float Phase = 0f;

    [Tooltip("The axis along which the object oscillates.")]
    public Vector3 WaveAxis = Vector3.right;

    float time;
    void Update()
    {
        time += Time.deltaTime;
        transform.position += ForwardSpeed * Time.deltaTime * transform.forward;

        float wave = Amplitude * Mathf.Cos((time * Frequency) + Phase);
        transform.position += transform.TransformDirection(WaveAxis.normalized) * wave;
    }
}
