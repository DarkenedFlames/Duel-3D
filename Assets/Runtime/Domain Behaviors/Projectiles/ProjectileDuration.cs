public class ProjectileDuration : ProjectileBehavior<ProjectileDurationDefinition>
{
    private float remainingTime;
    public ProjectileDuration(ProjectileDurationDefinition def, Projectile owner) : base(def, owner) { }
    public override void OnSpawn() => remainingTime = Definition.duration;
    public override void OnUpdate(float deltaTime)
    {
        remainingTime -= deltaTime;
        if (remainingTime <= 0f) Owner.Handler.Expire();
    }
}
