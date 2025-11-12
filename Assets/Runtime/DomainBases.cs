using UnityEngine;



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
