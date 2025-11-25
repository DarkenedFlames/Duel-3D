using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class HomingSequentialMover : IRegionMover
{
    [Header("Homing Settings")]
    [Tooltip("The global character runtime set for targeting.")]
    public CharacterSet allCharacters;

    [Tooltip("The rate (degrees/second) at which the region turns towards its target.")]
    public float TurnRate = 360f;

    [Tooltip("The speed (meters/second) at which the region moves.")]
    public float Speed = 10f;

    [Tooltip("The distance (meters) at which the region may reacquire homing targets.")]
    public float HomingDistance = 20f;

    [Tooltip("Distance at which the region is considered to have 'hit' its target.")]
    public float HitDistance = 0.5f;

    // State
    private Character currentTarget;
    private HashSet<Character> previousTargets = new();

    public IRegionMover Clone()
    {
        HomingSequentialMover copy = (HomingSequentialMover)MemberwiseClone();
        copy.currentTarget = null;
        copy.previousTargets = new();
        return copy;
    }

    public void Tick(Region region)
    {
        // Acquire new target when needed
        if (currentTarget == null)
        {
            TryAcquire(region);
        }
        else
        {
            UpdateHoming(region);
            float dist = Vector3.Distance(region.transform.position, currentTarget.transform.position);

            if (dist <= HitDistance)
            {
                previousTargets.Add(currentTarget);
                currentTarget = null;
            }
        }

        region.transform.position += Speed * Time.deltaTime * region.transform.forward;
    }

    private void TryAcquire(Region region)
    {
        List<Character> excluded = previousTargets.ToList();
        SpawnContext spawnContext = region.GetComponent<SpawnContext>();

        if (spawnContext.Owner != null && spawnContext.Owner.TryGetComponent(out Character ownerCharacter))
            excluded.Add(ownerCharacter);

        Character best = allCharacters.GetClosestExcludingMany(
            region.transform.position,
            excluded,
            out float distance
        );

        if (best != null && distance <= HomingDistance)
            currentTarget = best;
    }

    private void UpdateHoming(Region region)
    {
        if (currentTarget == null) return;

        Vector3 dir = (currentTarget.transform.position - region.transform.position).normalized;

        region.transform.forward = Vector3.RotateTowards(
            region.transform.forward,
            dir,
            TurnRate * Mathf.Deg2Rad * Time.deltaTime,
            0f
        );
    }
}
