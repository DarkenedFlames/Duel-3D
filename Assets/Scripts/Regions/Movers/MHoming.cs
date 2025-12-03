using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(IActionSource))]
public class MHoming : MonoBehaviour
{
    [Header("Homing Settings")]
    [Tooltip("The global character runtime set for targeting."), SerializeField]
    CharacterSet allCharacters;

    [Tooltip("Whether or not the source Character is targeted for homing."), SerializeField]
    bool targetsOwner = false;

    [Tooltip("The rate (degrees/second) at which the region turns towards its target."), SerializeField, Min(0)]
    float TurnRate = 360f;

    [Tooltip("The speed (meters/second) at which the region moves."), SerializeField, Min(0)]
    float Speed = 10f;

    [Tooltip("The distance (meters) at which the region may reacquire homing targets."), SerializeField, Min(0)]
    float HomingDistance = 20f;

    Character currentTarget;
    Character owner;

    void Start()
    {
        if (allCharacters == null || allCharacters.Count() == 0)
            LogFormatter.LogNullCollectionField(nameof(allCharacters), nameof(Start), nameof(MHoming), GetComponent<IActionSource>().GameObject);

        if (!TryGetComponent(out IActionSource source))
        {
            LogFormatter.LogMissingComponent(nameof(IActionSource), nameof(MHoming), gameObject);
            return;
        }
        if (source.Owner == null)
        {
            Debug.LogError($"{nameof(MHoming)} was given to an object with no owner!");
            return;
        }

        owner = source.Owner;
    }

    void Update()
    {
        if (currentTarget == null)
            TryAcquire();
        else
            UpdateHoming();
        
        transform.position += Speed * Time.deltaTime * transform.forward;
    }
    void TryAcquire()
    {
        List<Character> excluded = new();

        if (owner != null && !targetsOwner)
            excluded.Add(owner);

        Character best = allCharacters.GetClosestExcludingMany(
            transform.position,
            excluded,
            out float distance
        );
        
        if (best != null && distance <= HomingDistance) currentTarget = best;
        else currentTarget = null;
    }

    void UpdateHoming()
    {
        Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
        transform.forward = Vector3.RotateTowards(
            transform.forward,
            direction,
            TurnRate * Mathf.Deg2Rad * Time.deltaTime,
            0f
        );

        if (Vector3.Distance(transform.position, currentTarget.transform.position) > HomingDistance)
            currentTarget = null;
    }
}
