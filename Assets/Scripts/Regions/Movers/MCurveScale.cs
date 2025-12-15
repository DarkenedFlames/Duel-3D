using UnityEngine;
using System;

public class MCurveScale : MonoBehaviour
{
    [Flags]
    public enum Pin
    {
        None      = 0,
        PositiveX = 1 << 0,
        NegativeX = 1 << 1,
        PositiveY = 1 << 2,
        NegativeY = 1 << 3,
        PositiveZ = 1 << 4,
        NegativeZ = 1 << 5
    }

    [Header("Scale Curves (per axis)")]
    [SerializeField] AnimationCurve X = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] AnimationCurve Y = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] AnimationCurve Z = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Pin Configuration")]
    [SerializeField] Pin pins;

    [Header("Scale Settings")]
    [SerializeField] Vector3 initialScale = Vector3.one;
    [SerializeField] Vector3 finalScale = Vector3.one * 2f;

    [SerializeField] float duration = 1f;

    FloatCounter seconds;
    Vector3 previousOffset;

    void Start()
    {
        seconds = new(0, 0, duration, resetToMax: false);
        previousOffset = Vector3.zero;
        transform.localScale = initialScale;
    }

    void Update()
    {
        seconds.Increase(Time.deltaTime);
        float progress = seconds.Progress;

        float curveX = X.Evaluate(progress);
        float curveY = Y.Evaluate(progress);
        float curveZ = Z.Evaluate(progress);

        Vector3 currentScale = new(
            Mathf.Lerp(initialScale.x, finalScale.x, curveX),
            Mathf.Lerp(initialScale.y, finalScale.y, curveY),
            Mathf.Lerp(initialScale.z, finalScale.z, curveZ)
        );

        Vector3 dS = currentScale - initialScale;
        Vector3 offset = Vector3.zero;

        if ((pins & Pin.PositiveX) != 0) offset.x -= 0.5f * dS.x;
        else if ((pins & Pin.NegativeX) != 0) offset.x += 0.5f * dS.x;

        if ((pins & Pin.PositiveY) != 0) offset.y -= 0.5f * dS.y;
        else if ((pins & Pin.NegativeY) != 0) offset.y += 0.5f * dS.y;

        if ((pins & Pin.PositiveZ) != 0) offset.z -= 0.5f * dS.z;
        else if ((pins & Pin.NegativeZ) != 0) offset.z += 0.5f * dS.z;

        transform.localScale = currentScale;
        
        Vector3 deltaOffset = offset - previousOffset;
        Vector3 localDeltaOffset = transform.localRotation * deltaOffset;
        transform.localPosition += localDeltaOffset;
        previousOffset = offset;
    }
}