using UnityEngine;

[CreateAssetMenu(fileName = "New Area Modify Effect Behavior", menuName = "Duel/Areas/ModifyEffect")]
public class AreaModifyEffectDefinition : AreaBehaviorDefinition
{
    [Tooltip("Period (s) of effect application or removal for OnTick.")]
    public float period = 1f;

    [Header("Effect Configs")]
    public EffectConfig[] sourceConfigs;
    public EffectConfig[] targetConfigs;
    public EffectConfig[] otherConfigs;

    public override AreaBehavior CreateRuntimeBehavior(Area area)
    {
        return new AreaModifyEffect(area, this);
    }
}

public class AreaModifyEffect : AreaBehavior
{
    private new AreaModifyEffectDefinition Definition => (AreaModifyEffectDefinition)base.Definition;
    private float _pulseTimer;

    public AreaModifyEffect(Area area, AreaModifyEffectDefinition definition) : base(area, definition) { }

    private EffectConfig[] GetConfigsByActor(GameObject actor)
    {
        if (actor == Area.sourceActor) return Definition.sourceConfigs;
        else if (actor == Area.targetActor) return Definition.targetConfigs;
        else return Definition.otherConfigs;
    }

    private void ModifyEffect(GameObject target, bool mode, EffectDefinition def, int stacks)
    {
        if (target.TryGetComponent(out EffectHandler handler))
            if (mode)
                handler.ApplyEffect(def, stacks);
            else
                handler.RemoveStacks(def.effectName, stacks);
    }

    public override void OnTargetEnter(GameObject target)
    {
        foreach (EffectConfig config in GetConfigsByActor(target))
            if (config.hookType.HasFlag(HookType.OnTargetEnter))
                ModifyEffect(target, config.mode, config.effectDefinition, config.stacks);
        
    }
    public override void OnTargetExit(GameObject target)
    {
        foreach (EffectConfig config in GetConfigsByActor(target))
            if (config.hookType.HasFlag(HookType.OnTargetExit))
                ModifyEffect(target, config.mode, config.effectDefinition, config.stacks);
    }

    public override void OnTick(float deltaTime)
    {
        _pulseTimer += deltaTime;
        if (_pulseTimer >= Definition.period)
        {
            _pulseTimer = 0f;
            foreach (GameObject target in Area.CurrentTargets)
                foreach (EffectConfig config in GetConfigsByActor(target))
                    if (config.hookType.HasFlag(HookType.OnTick))
                        ModifyEffect(target, config.mode, config.effectDefinition, config.stacks);
        }
    }
    
    public override void OnExpire()
    {
        foreach (GameObject target in Area.CurrentTargets)
            foreach (EffectConfig config in GetConfigsByActor(target))
                if (config.hookType.HasFlag(HookType.OnExpire))
                    ModifyEffect(target, config.mode, config.effectDefinition, config.stacks);
    }
}
