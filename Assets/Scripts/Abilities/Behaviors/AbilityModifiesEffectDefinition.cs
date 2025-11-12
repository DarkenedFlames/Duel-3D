using UnityEngine;

[CreateAssetMenu(fileName = "New Ability Modify Effect Behavior", menuName = "Duel/Abilities/Behaviors/ModifyEffect")]
public class AbilityModifyEffectDefinition : AbilityBehaviorDefinition
{
    [Header("Effect Configs")]
    public EffectConfig[] configs;

    public override AbilityBehavior CreateRuntimeBehavior() => new AbilityModifyEffect(this);
}

public class AbilityModifyEffect : AbilityBehavior
{
    readonly AbilityModifyEffectDefinition def;
    public AbilityModifyEffect(AbilityModifyEffectDefinition d) => def = d;
    
    private void ModifyEffect(HookType type)
    {
        foreach (EffectConfig config in def.configs)
            if (config.hookType.HasFlag(type))
                if (Execution.Handler.TryGetComponent(out EffectHandler handler))
                    if (config.mode)
                        handler.ApplyEffect(config.effectDefinition, config.stacks);
                    else
                        handler.RemoveStacks(config.effectDefinition.effectName, config.stacks);
    }

    public override void OnActivate() => ModifyEffect(HookType.OnActivate);
}
