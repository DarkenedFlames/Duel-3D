using UnityEngine;

public abstract class ProjectileBehaviorDefinition : ScriptableObject
{
    public abstract ProjectileBehavior CreateRuntimeBehavior(Projectile projectile);
}

public abstract class ProjectileBehavior
{
    protected Projectile Projectile;
    protected ProjectileBehaviorDefinition Definition;

    public ProjectileBehavior(Projectile projectile, ProjectileBehaviorDefinition definition)
    {
        Projectile = projectile;
        Definition = definition;
    }

    public virtual void OnTick(float deltaTime) { }
    public virtual void OnCollide(GameObject target) { }
    public virtual void OnExpire() { }
}