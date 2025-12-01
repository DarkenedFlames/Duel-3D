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

    Character currentTarget;
    readonly HashSet<Character> previousTargets = new();

    void Start()
    {
        if (allCharacters == null || allCharacters.Count() == 0)
            LogFormatter.LogNullField(nameof(allCharacters), nameof(MHoming), GetComponent<IActionSource>().GameObject);
    }

    void Update()
    {
        if (currentTarget == null) TryAcquire();
        else
        {
            UpdateHoming();
            float dist = Vector3.Distance(transform.position, currentTarget.transform.position);

            if (dist <= HitDistance)
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

        Character owner = GetComponent<IActionSource>().Owner;
        if (owner == null)
        {
            Debug.LogError($"Mover {nameof(MHomingSequential)} found a null {nameof(IActionSource.Owner)}", GetComponent<IActionSource>().GameObject);
            return;
        }

        if (!targetsSource)
            excluded.Add(owner);

        Character best = allCharacters.GetClosestExcludingMany(
            transform.position,
            excluded,
            out float distance
        );

        if (best != null && distance <= HomingDistance)
            currentTarget = best;
    }

    void UpdateHoming()
    {
        if (currentTarget == null) return;

        Vector3 dir = (currentTarget.transform.position - transform.position).normalized;

        transform.forward = Vector3.RotateTowards(
            transform.forward,
            dir,
            TurnRate * Mathf.Deg2Rad * Time.deltaTime,
            0f
        );
    }
}
