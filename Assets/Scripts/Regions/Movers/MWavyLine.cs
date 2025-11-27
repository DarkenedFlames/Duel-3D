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

    Vector3 initialPos;
    float time;
    void Start()
    {
        initialPos = transform.position;

        if (ForwardSpeed <= 0)
            Debug.LogError($"{name}'s Mover {nameof(MWavyLine)} was configured with an invalid parameter: {nameof(ForwardSpeed)} must be positive!");
        if (Mathf.Approximately(Amplitude, 0))
            Debug.LogError($"{name}'s Mover {nameof(MWavyLine)} was configured with an invalid parameter: {nameof(Amplitude)} must be non-zero (might be too small)!");
        if (Frequency <= 0)
            Debug.LogError($"{name}'s Mover {nameof(MWavyLine)} was configured with an invalid parameter: {nameof(Frequency)} must be positive!");
    }

    void Update()
    {
        time += Time.deltaTime;
        Vector3 forwardMovement = ForwardSpeed * time * transform.forward;

        float wave = Amplitude * Mathf.Cos((time * Frequency) + Phase);
        Vector3 waveOffset = transform.TransformDirection(WaveAxis.normalized) * wave;

        transform.position = initialPos + forwardMovement + waveOffset;
    }
}
