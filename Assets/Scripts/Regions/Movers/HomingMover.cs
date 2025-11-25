using System;
using System.Collections.Generic;
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

    Character currentTarget;
    public void Tick(Region region)
    {
        if (currentTarget == null)
        {
            Character best;
            float trueDistance;
            
            if (region.Definition.AffectsSource)
            {
                best = allCharacters.GetClosest(region.transform.position, out float distance);
                trueDistance = distance;
            }
            else
            {
                Character sourceCharacter = region.GetComponent<SpawnContext>().Owner.GetComponent<Character>();
                best = allCharacters.GetClosestExcluding(region.transform.position, sourceCharacter, out float distance);
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
                TurnRate * Time.deltaTime,
                0f
            );

            if (Vector3.Distance(region.transform.position, currentTarget.transform.position) > HomingDistance)
                currentTarget = null;
        }

        region.transform.position += Speed * Time.deltaTime * region.transform.forward;
    }
}
