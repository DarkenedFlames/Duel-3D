using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MovementType
{
    None,
    Physics,
    Straight,
    RelentlessHoming,
    ForgivingHoming,
    Path
}

[RequireComponent(typeof(Rigidbody))]
public class NonActorController : MonoBehaviour
{
    [Header("General Settings")]
    public MovementType movementType = MovementType.None;
    public float speed = 10f;
    public float turnSpeed = 360f;

    [Header("Homing Settings")]
    public float homingRange = 15f; // For ForgivingHoming only
    public bool sourceIsTargetable = false;

    private Rigidbody rb;
    public GameObject HomingTarget { get; private set; }
    private GameObject SourceActor
    {
        get
        {
            return TryGetComponent(out IHasSourceActor hasSource)
                ? hasSource.SourceActor
                : null;
        }
    }
    private Vector3 currentDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;
        currentDirection = transform.forward;
    }

    void FixedUpdate()
    {
        switch (movementType)
        {
            case MovementType.None:
                rb.linearVelocity = Vector3.zero;
                break;

            case MovementType.Physics:
                rb.useGravity = true;
                rb.linearVelocity = transform.forward * speed;
                break;

            case MovementType.Straight:
                rb.useGravity = false;
                rb.linearVelocity = transform.forward * speed;
                break;

            case MovementType.RelentlessHoming:
                rb.useGravity = false;
                HandleHoming(persistent: true);
                break;

            case MovementType.ForgivingHoming:
                rb.useGravity = false;
                HandleHoming(persistent: false);
                break;
        }
    }

    private void HandleHoming(bool persistent)
    {
        // Lose target if out of range (ForgivingHoming only)
        if (!persistent && HomingTarget != null)
        {
            float distance = Vector3.Distance(transform.position, HomingTarget.transform.position);
            if (distance > homingRange)
                HomingTarget = null;
        }

        // Reacquire if Relentless and lost target
        // This is a good check, otherwise we would constantly query for new targets
        if (HomingTarget == null && persistent)
            AcquireClosestTarget();

        // Turn toward target
        if (HomingTarget != null)
        {
            Vector3 dir = (HomingTarget.transform.position - transform.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRot,
                turnSpeed * Time.fixedDeltaTime
            );

            currentDirection = transform.forward;
        }
        else
        {
            // Keep going straight
            transform.rotation = Quaternion.LookRotation(currentDirection);
        }

        rb.linearVelocity = currentDirection * speed;
    }

    private void AcquireClosestTarget()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        HashSet<GameObject> potentialTargets = allObjects
                .Select(go => go.transform.root.gameObject)
                .Where(go => go.layer == LayerMask.NameToLayer("Actors"))
                .ToHashSet();

        if (potentialTargets.Count == 0) return;

        Transform closest = null;
        float closestDist = float.MaxValue;
        Vector3 pos = transform.position;

        foreach (GameObject target in potentialTargets)
        {
            if (target == null) continue;
            if (!sourceIsTargetable && target == SourceActor) continue;

            float dist = Vector3.SqrMagnitude(target.transform.position - pos);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = target.transform;
            }
        }

        HomingTarget = closest ? closest.gameObject : null;
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (movementType == MovementType.ForgivingHoming)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, homingRange);
        }
    }
    #endif
}