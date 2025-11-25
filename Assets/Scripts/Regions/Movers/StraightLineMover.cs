using System;
using UnityEngine;

[Serializable]
public class StraightLineMover : IRegionMover
{
    [Header("The speed (meters/second) at which the region moves.")]
    public float Speed;
    
    public IRegionMover Clone() => (StraightLineMover)MemberwiseClone();
    public void Tick(Region region)
    {
        region.transform.position += Speed * Time.deltaTime * region.transform.forward;
    }
}