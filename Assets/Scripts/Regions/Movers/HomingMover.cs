using System;
using UnityEngine;

[Serializable]
public class HomingMover : IRegionMover
{
    [Header("Homing Settings")]
    [Tooltip("The global character runtime set for targeting.")]
    public CharacterSet allCharacters;

    [Tooltip("The rate (degrees/second) at which the region turns towards its target.")]
    public float TurnRate;

    [Tooltip("The speed (meters/second) at which the region moves.")]
    public float Speed;

    [Tooltip("The distance (meters) at which the region may reacquire homing targets.")]
    public float HomingDistance;

    // State
    Character currentTarget;

    public IRegionMover Clone()
    {
        HomingMover copy = (HomingMover)MemberwiseClone();
        copy.currentTarget = null;
        return copy;
    }

    public void Tick(Region region)
    {
        if (currentTarget == null)
        {
            Character best;
            float trueDistance;
            GameObject ownerObject = region.GetComponent<SpawnContext>().Owner;
            
            if (ownerObject != null && !region.Definition.AffectsSource && ownerObject.TryGetComponent(out Character character))
            {
                best = allCharacters.GetClosestExcluding(region.transform.position, character, out float distance);
                trueDistance = distance;
            }
            else
            {
                best = allCharacters.GetClosest(region.transform.position, out float distance);
                trueDistance = distance;
            }
            
            if (best != null && trueDistance <= HomingDistance) currentTarget = best;
            else currentTarget = null;
        }
        else
        {
            Vector3 direction = (currentTarget.transform.position - region.transform.position).normalized;
            region.transform.forward = Vector3.RotateTowards(
                region.transform.forward,
                direction,
                TurnRate * Mathf.Deg2Rad * Time.deltaTime,
                0f
            );

            if (Vector3.Distance(region.transform.position, currentTarget.transform.position) > HomingDistance)
                currentTarget = null;
        }

        region.transform.position += Speed * Time.deltaTime * region.transform.forward;
    }
}
