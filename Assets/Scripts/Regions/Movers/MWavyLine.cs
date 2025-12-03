using UnityEngine;
public class MWavyLine : MonoBehaviour
{
    [Header("Wavy Line Settings")]
    [Tooltip("The speed (meters/second) at which the object moves foward."), SerializeField, Min(0)]
    float ForwardSpeed = 8f;

    [Tooltip("The range (meters) at which the object strafes."), SerializeField, Min(0)]
    float Amplitude = 1f;

    [Tooltip("The frequency (Hz) at which the wavy path repeats."), SerializeField, Min(0)]
    float Frequency = 2f;

    [Tooltip("The offset (radians) within the path at which the object starts."), SerializeField, Range(0,360)]
    float Phase = 0f;

    [Tooltip("The axis along which the object oscillates."), SerializeField]
    Vector3 WaveAxis = Vector3.right;

    Vector3 initialPos;
    float time;
    void Start()
    {
        initialPos = transform.position;
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
