using UnityEngine;
using System;

[Serializable]
public class LinearScaleMover : IRegionMover
{
    public Vector3 TargetScale = Vector3.one * 2f;
    public float ScaleRate = 1f;

    public IRegionMover Clone() => (LinearScaleMover)MemberwiseClone();
    public void Tick(Region region)
    {
        region.transform.localScale = Vector3.MoveTowards(
            region.transform.localScale,
            TargetScale,
            ScaleRate * Time.deltaTime
        );
    }
}
