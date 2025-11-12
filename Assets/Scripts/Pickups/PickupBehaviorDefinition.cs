using UnityEngine;

public abstract class PickupBehaviorDefinition : ScriptableObject
{
    public abstract PickupBehavior CreateRuntimeBehavior(Pickup pickup);
}

public abstract class PickupBehavior
{
    protected Pickup Pickup;
    protected PickupBehaviorDefinition Definition;

    public PickupBehavior(Pickup pickup, PickupBehaviorDefinition definition)
    {
        Pickup = pickup;
        Definition = definition;
    }

    public virtual void OnCollide(GameObject target) { }
    public virtual void OnExpire() { }
}