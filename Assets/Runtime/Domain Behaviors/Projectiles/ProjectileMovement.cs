using UnityEngine;

public class ProjectileMovement : ProjectileBehavior<ProjectileMovementDefinition>
{
    private Transform target;

    public ProjectileMovement(ProjectileMovementDefinition def, Projectile owner) : base(def, owner) { }

    public override void OnUpdate(float deltaTime)
    {
        ProjectileHandler handler = Owner.Handler;
        var rb = handler.GetComponent<Rigidbody>();

        switch (Definition.movementType)
        {
            case ProjectileMovementType.Straight:
                rb.useGravity = false;
                rb.linearVelocity = handler.transform.forward * Definition.speed;
                break;

            case ProjectileMovementType.Gravity:
                rb.useGravity = true;
                rb.linearVelocity = handler.transform.forward * Definition.speed; 
                break;

            case ProjectileMovementType.Homing:
                if (target == null) AcquireTarget(handler); // Try to re-acquire target
                if (target != null)
                {
                    Vector3 dir = (target.position - handler.transform.position).normalized;
                    Quaternion targetRot = Quaternion.LookRotation(dir);
                    handler.transform.rotation = Quaternion.RotateTowards(
                        handler.transform.rotation,
                        targetRot,
                        Definition.turnSpeed * deltaTime
                    );
                }

                rb.linearVelocity = handler.transform.forward * Definition.speed;
                break;
        }
    }

    private void AcquireTarget(ProjectileHandler handler)
    {
        StatsHandler[] potentialTargets = Object.FindObjectsByType<StatsHandler>(FindObjectsSortMode.None);
        Transform closest = null;
        float closestDist = Mathf.Infinity;
        Vector3 pos = handler.transform.position;

        foreach (var stats in potentialTargets)
        {
            if (stats.gameObject == handler.source) continue; // skip source

            float dist = Vector3.Distance(stats.transform.position, pos);
            if (dist < closestDist && dist <= Definition.homingRange)
            {
                closestDist = dist;
                closest = stats.transform;
            }
        }

        target = closest;
    }
}
