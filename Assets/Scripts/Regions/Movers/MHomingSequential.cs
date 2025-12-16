using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(IActionSource))]
public class MHomingSequential : MonoBehaviour
{
    [Header("Homing Settings")]
    [Tooltip("The global character runtime set for targeting."), SerializeField]
    CharacterSet allCharacters;

    [Tooltip("Whether or not the source Character is targeted for homing."), SerializeField]
    bool targetsSource = false;

    [Tooltip("The rate (degrees/second) at which the region turns towards its target."), SerializeField, Min(0)]
    float TurnRate = 360f;

    [Tooltip("The speed (meters/second) at which the region moves."), SerializeField, Min(0)]
    float Speed = 10f;

    [Tooltip("The distance (meters) at which the region may reacquire homing targets."), SerializeField, Min(0)]
    float HomingDistance = 20f;

    [Tooltip("The distance (meters) at which the region is considered to have 'hit' its target."), SerializeField, Min(0)]
    float HitDistance = 0.5f;

    [Tooltip("Offset from the target's position to aim at (e.g., 0,1,0 for chest height)."), SerializeField]
    Vector3 TargetOffset = new(0, 1, 0);

    Character owner;
    Character currentTarget;
    readonly HashSet<Character> previousTargets = new();

    float GetTargetDistance() => Vector3.Distance(transform.position, GetTargetPosition());
    Vector3 GetTargetDirection() => (GetTargetPosition() - transform.position).normalized;
    Vector3 GetTargetPosition() => currentTarget.transform.position + TargetOffset;


    void Start()
    {
        if (allCharacters == null || allCharacters.Count() == 0)
            LogFormatter.LogNullCollectionField(nameof(allCharacters), nameof(Start), nameof(MHomingSequential), GetComponent<IActionSource>().GameObject);

        if (!TryGetComponent(out IActionSource source))
        {
            LogFormatter.LogMissingComponent(nameof(IActionSource), nameof(MHomingSequential), gameObject);
            return;
        }

        owner = source.Owner;
    }

    void Update()
    {
        if (currentTarget == null) TryAcquire();
        else
        {
            transform.forward = Vector3.RotateTowards(
                transform.forward,
                GetTargetDirection(),
                TurnRate * Mathf.Deg2Rad * Time.deltaTime,
                0f
            );

            if (GetTargetDistance() <= HitDistance)
            {
                previousTargets.Add(currentTarget);
                currentTarget = null;
            }
        }

        transform.position += Speed * Time.deltaTime * transform.forward;
    }
    
    void TryAcquire()
    {
        List<Character> excluded = previousTargets.ToList();

        if (owner != null && !targetsSource)
            excluded.Add(owner);

        Character best = allCharacters.GetClosestExcludingMany(
            transform.position,
            excluded,
            out float distance
        );

        if (best != null && distance <= HomingDistance)
            currentTarget = best;
    }
}
