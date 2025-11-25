using System;
using UnityEngine;

[Serializable]
public class StraightLineMover : IRegionMover
{
    [Header("The speed (meters/second) at which the region moves.")]
    public float Speed;
    
    public void Tick(Region region)
    {
        GameObject spawner = region.GetComponent<SpawnContext>().Spawner;
        Vector3 direction = (spawner.transform.position - region.transform.position).normalized;
        region.transform.position += Speed * Time.deltaTime * direction;
    }
}