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
    public AreaModifyEffect(Area area, AreaModifyEffectDefinition definition) : base(area, definition) { }
    
    private float _pulseTimer;
    private void ModifyEffect(GameObject actor, HookType type)
    {
        EffectConfig[] configs = actor switch
        {
            _ when actor == Area.sourceActor => Definition.sourceConfigs,
            _ when actor == Area.targetActor => Definition.targetConfigs,
            _ => Definition.otherConfigs
        };

        foreach (EffectConfig config in configs)
            if (config.hookType.HasFlag(type))
                if (actor.TryGetComponent(out EffectHandler handler))
                    if (config.mode)
                        handler.ApplyEffect(config.effectDefinition, config.stacks);
                    else
                        handler.RemoveStacks(config.effectDefinition.effectName, config.stacks);
    }

    public override void OnTargetEnter(GameObject target) => ModifyEffect(target, HookType.OnTargetEnter);
    public override void OnTargetExit(GameObject target) => ModifyEffect(target, HookType.OnTargetExit);

    public override void OnTick(float deltaTime)
    {
        _pulseTimer += deltaTime;
        if (_pulseTimer < Definition.period) return;
        
        _pulseTimer = 0f;
        foreach (GameObject actor in Area.CurrentTargets)
            ModifyEffect(actor, HookType.OnTick);
    }
    
    public override void OnExpire()
    {
        foreach (GameObject actor in Area.CurrentTargets)
            ModifyEffect(actor, HookType.OnTick);
    }
}
