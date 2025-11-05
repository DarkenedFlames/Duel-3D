using UnityEngine;

public class ProjectileKnockback : ProjectileBehavior<ProjectileKnockbackDefiniton>
{
    public ProjectileKnockback(ProjectileKnockbackDefiniton def, Projectile owner) : base(def, owner) { }

    public override void OnTrigger(Collider other)
    {
        if(other.TryGetComponent(out Rigidbody rb))
        {
            Vector3 direction = other.ClosestPointOnBounds(Owner.Handler.transform.position) - Owner.Handler.transform.position;
            direction.Normalize();
            rb.AddForce(direction * Definition.force, ForceMode.Impulse);
        }
    }
}