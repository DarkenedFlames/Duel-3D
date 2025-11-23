using System;
using UnityEngine;

[Serializable]
public class HomingMover : IRegionMover
{
    float turnRate;
    float speed;
    public void Tick(Region region)
    {
        /*
        // Needs to query for nearest target
        if (target != null)
        {
            Vector3 dir = (target.position - region.transform.position).normalized;
            region.transform.forward = Vector3.RotateTowards(
                region.transform.forward, dir, turnRate * Time.deltaTime, 0f);
        }

        region.transform.position += speed * Time.deltaTime * region.transform.forward;
        */
    }
}