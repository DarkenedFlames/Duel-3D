using UnityEngine;

[CreateAssetMenu(fileName = "New Pickup Duration Behavior", menuName = "Pickups/Behaviors/Duration")]
public class PickupDurationDefinition : PickupBehaviorDefinition<PickupDuration, PickupDurationDefinition>
{
    [Header("Duration Settings")]
    public float duration = 5f;
    protected override PickupDuration CreateTypedInstance(Pickup owner) => new(this, owner);
}