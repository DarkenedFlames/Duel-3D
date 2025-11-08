using UnityEngine;

public abstract class AreaBehaviorDefinition : ScriptableObject
{
    public abstract AreaBehavior CreateRuntimeBehavior(Area area);
}

public abstract class AreaBehavior
{
    protected Area Area;
    protected AreaBehaviorDefinition Definition;

    public AreaBehavior(Area area, AreaBehaviorDefinition definition)
    {
        Area = area;
        Definition = definition;
    }

    public virtual void OnStart() { }
    public virtual void OnTick(float deltaTime) { }
    public virtual void OnTargetEnter(Collider target) { }
    public virtual void OnTargetExit(Collider target) { }
    public virtual void OnExpire() { }
}