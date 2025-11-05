using UnityEngine;

public class ProjectileDamage : ProjectileBehavior<ProjectileDamageDefinition>
{
    public ProjectileDamage(ProjectileDamageDefinition def, Projectile owner) : base(def, owner) { }

    public override void OnTrigger(Collider other)
    {
        if (other.gameObject.TryGetComponent(out StatsHandler stats)) stats.TakeDamage(Definition.impactDamage);
    }
}