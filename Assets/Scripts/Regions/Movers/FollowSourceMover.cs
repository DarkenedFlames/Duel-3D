using UnityEngine;
using System;

[Serializable]
public class FollowSourceMover : IRegionMover
{
    public Vector3 LocalOffset;

    // No state
    public IRegionMover Clone() => (FollowSourceMover)MemberwiseClone();
    public void Tick(Region region)
    {
        SpawnContext spawnContext = region.GetComponent<SpawnContext>();
        GameObject source = spawnContext.Owner != null ? spawnContext.Spawner : spawnContext.Owner;

        if (source == null) return;

        region.transform.SetPositionAndRotation(source.transform.TransformPoint(LocalOffset), source.transform.rotation);
    }
}
