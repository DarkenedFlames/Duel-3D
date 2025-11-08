using UnityEngine;




// Projectile
public abstract class ProjectileBehavior
{
    public virtual void OnSpawn() { }
    public virtual void OnUpdate(float deltaTime) { }
    public virtual void OnExpire() { }
    public virtual void OnTrigger(Collider collider) { }
}
public abstract class ProjectileBehaviorDefinition : ScriptableObject
{
    public abstract ProjectileBehavior CreateRuntimeInstance(Projectile owner);
}

// Weapon
public abstract class WeaponBehavior
{
    public virtual void OnAttack() { }
    public virtual void OnTrigger(Collider other) { }
    public virtual void OnUpdate(float deltaTime) { }
    public virtual void OnSpawn() { }
}
public abstract class WeaponBehaviorDefinition : ScriptableObject
{
    public abstract WeaponBehavior CreateRuntimeInstance(Weapon owner);
}

// Pickup
public abstract class PickupBehavior
{
    public virtual void OnSpawn() { }
    public virtual void OnTrigger(Collider other) { }
    public virtual void OnExpire() { }
    public virtual void OnUpdate(float deltaTime) { }
}
public abstract class PickupBehaviorDefinition : ScriptableObject
{
    public abstract PickupBehavior CreateRuntimeInstance(Pickup owner);
}


public abstract class ProjectileBehavior<TDefinition> : ProjectileBehavior
    where TDefinition : ProjectileBehaviorDefinition
{
    protected readonly TDefinition Definition;
    protected readonly Projectile Owner;

    protected ProjectileBehavior(TDefinition definition, Projectile owner)
    {
        Definition = definition;
        Owner = owner;
    }
}

public abstract class ProjectileBehaviorDefinition<TBehavior, TDefinition> : ProjectileBehaviorDefinition
    where TBehavior : ProjectileBehavior<TDefinition>
    where TDefinition : ProjectileBehaviorDefinition<TBehavior, TDefinition>
{
    public override ProjectileBehavior CreateRuntimeInstance(Projectile owner) => CreateTypedInstance(owner);
    protected abstract TBehavior CreateTypedInstance(Projectile owner);
}

// Weapon
public abstract class WeaponBehavior<TDefinition> : WeaponBehavior
    where TDefinition : WeaponBehaviorDefinition
{
    protected readonly TDefinition Definition;
    protected readonly Weapon Owner;

    protected WeaponBehavior(TDefinition definition, Weapon owner)
    {
        Definition = definition;
        Owner = owner;
    }
}

public abstract class WeaponBehaviorDefinition<TBehavior, TDefinition> : WeaponBehaviorDefinition
    where TBehavior : WeaponBehavior<TDefinition>
    where TDefinition : WeaponBehaviorDefinition<TBehavior, TDefinition>
{
    public override WeaponBehavior CreateRuntimeInstance(Weapon owner) => CreateTypedInstance(owner);
    protected abstract TBehavior CreateTypedInstance(Weapon owner);
}

// Pickup
public abstract class PickupBehavior<TDefinition> : PickupBehavior
    where TDefinition : PickupBehaviorDefinition
{
    protected readonly TDefinition Definition;
    protected readonly Pickup Owner;

    protected PickupBehavior(TDefinition definition, Pickup owner)
    {
        Definition = definition;
        Owner = owner;
    }
}

public abstract class PickupBehaviorDefinition<TBehavior, TDefinition> : PickupBehaviorDefinition
    where TBehavior : PickupBehavior<TDefinition>
    where TDefinition : PickupBehaviorDefinition<TBehavior, TDefinition>
{
    public override PickupBehavior CreateRuntimeInstance(Pickup owner) => CreateTypedInstance(owner);
    protected abstract TBehavior CreateTypedInstance(Pickup owner);
}
