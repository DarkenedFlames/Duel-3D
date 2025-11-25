using UnityEngine;
using HBM.Scriptable;

[CreateAssetMenu(menuName = "Sets/Region")]
public class RegionSet : RuntimeSet<Region>
{
    public Region GetClosest(Vector3 position, out float distance)
    {
        distance = float.PositiveInfinity;
        Region closest = null;

        foreach (Region region in this)
        {
            if (region == null) continue;

            float newDistance = Vector3.Distance(region.transform.position, position);
            if (newDistance < distance)
            {
                distance = newDistance;
                closest = region;
            }
        }

        return closest;
    }

    public Region GetClosestExcluding(Vector3 position, Region exclude, out float distance)
    {
        Region closest = null;
        distance = float.MaxValue;

        foreach (Region region in this)
        {
            if (region == exclude) continue;

            float d = Vector3.Distance(position, region.transform.position);
            if (d < distance)
            {
                distance = d;
                closest = region;
            }
        }

        return closest;
    }
}
