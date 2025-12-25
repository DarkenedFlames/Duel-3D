using UnityEngine;

public class LightOrbitEntry : MonoBehaviour
{
    [Tooltip("Light component to orbit.")]
    public Light Light;
    
    [Tooltip("Color gradient throughout the day cycle.")]
    public Gradient ColorGradient = new();

    [Tooltip("Percentage intensity (intensity + color) curve of the light per rotation.")]
    public AnimationCurve IntensityCurve = new(new Keyframe(0, 1), new Keyframe(1, 1));

    [Tooltip("Angular offset in degrees."), Range(0, 360)]
    public float Offset = 0f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 10f);
    }
}