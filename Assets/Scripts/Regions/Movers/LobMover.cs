using UnityEngine;
using System;

[Serializable]
public class LobMover : IRegionMover
{
    public float Speed = 10f;

    public IRegionMover Clone() => (LobMover)MemberwiseClone();
    public void Tick(Region region)
    {
        Vector3 velocity = region.transform.forward * Speed;
        velocity += Physics.gravity * Time.deltaTime;  

        region.transform.position += velocity * Time.deltaTime;
        region.transform.forward = velocity.normalized;
    }
}
