using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpawnContext))]
public class MHoming : MonoBehaviour
{
    [Header("Homing Settings")]
    [Tooltip("The global character runtime set for targeting.")]
    public CharacterSet allCharacters;

    [Tooltip("Whether or not the source Character is targeted for homing.")]
    public bool targetsSource = false;

    [Tooltip("The rate (degrees/second) at which the region turns towards its target.")]
    public float TurnRate = 360f;

    [Tooltip("The speed (meters/second) at which the region moves.")]
    public float Speed = 10f;

    [Tooltip("The distance (meters) at which the region may reacquire homing targets.")]
    public float HomingDistance = 20f;

    Character currentTarget;

    void Start()
    {
        if (TurnRate <= 0)
            Debug.LogError($"{name}'s Mover {nameof(MHoming)} was configured with an invalid parameter: {nameof(TurnRate)} must be positive!");
        if (Speed <= 0)
            Debug.LogError($"{name}'s Mover {nameof(MHoming)} was configured with an invalid parameter: {nameof(Speed)} must be positive!");
        if (HomingDistance <= 0)
            Debug.LogError($"{name}'s Mover {nameof(MHoming)} was configured with an invalid parameter: {nameof(HomingDistance)} must be positive!");
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
        Character owner = GetComponent<SpawnContext>().Owner;
        List<Character> excluded = new();

        if (owner != null && !targetsSource)
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
