// File: AbilityBehavior.cs
public abstract class AbilityBehavior
{
    public AbilityBehaviorDefinition Definition { get; private set; }
    protected AbilityExecution Execution { get; private set; }
    public int Priority { get; protected set; } = 100;

    public void Initialize(AbilityBehaviorDefinition def, AbilityExecution exec)
    {
        Definition = def;
        Execution = exec;
        Priority = def.defaultPriority;
        OnInitialized();
    }

    // Called once on creation
    protected virtual void OnInitialized() { }

    // Quick check honoring required/forbidden tags on the execution's caster container
    public virtual bool IsEligible()
    {
        if (!Execution.MatchTag(Definition.requiredTags)) return false;
        if (Execution.HasTag(Definition.forbiddenTags)) return false;
        return true;
    }

    // Hooks called by the execution lifecycle
    public abstract void OnActivate();
    public virtual void OnTick(float dt) { }
    public virtual void OnEnd() { }
}
