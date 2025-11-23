using System;
using UnityEngine;

[Serializable]
public class StraightLineMover : IRegionMover
{
    float speed;
    public void Tick(Region region)
    {
        Vector3 direction = region.GetComponent<IRequiresSource>().Source.transform.position - region.transform.position;
        region.transform.position += speed * Time.deltaTime * direction.normalized;
    }
}