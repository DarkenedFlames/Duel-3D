public class PickupDuration : PickupBehavior<PickupDurationDefinition>
{
    public PickupDuration(PickupDurationDefinition def, Pickup owner) : base(def, owner) { }
    private float remainingDuration;

    public override void OnSpawn() => remainingDuration = Definition.duration;
    public override void OnUpdate(float deltaTime)
    {
        remainingDuration -= deltaTime;
        if (remainingDuration <= 0f) Owner.Handler.Expire();
    }
}
