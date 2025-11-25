using UnityEngine;
using System;

[Serializable]
public class WavyLineMover : IRegionMover
{
    public float ForwardSpeed = 8f;
    public float Amplitude = 1f;
    public float Frequency = 2f;
    public float Phase = 0f;
    public Vector3 WaveAxis = Vector3.right;

    float time;

    public IRegionMover Clone()
    {
        WavyLineMover copy = (WavyLineMover)MemberwiseClone();
        copy.time = 0;
        return copy;
    }
    public void Tick(Region region)
    {
        time += Time.deltaTime;

        region.transform.position += ForwardSpeed * Time.deltaTime * region.transform.forward;

        float wave = Amplitude * Mathf.Cos((time * Frequency) + Phase);

        region.transform.position += region.transform.TransformDirection(WaveAxis.normalized) * wave;
    }
}
