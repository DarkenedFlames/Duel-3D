using UnityEngine;

[CreateAssetMenu(fileName = "New Ability Modify Stat Behavior", menuName = "Duel/Abilities/Behaviors/ModifyStat")]
public class AbilityModifyStatDefinition : AbilityBehaviorDefinition
{
    [Header("Stat Configs")]
    public StatConfig[] configs;

    public override AbilityBehavior CreateRuntimeBehavior() => new AbilityModifyStat(this);
}

public class AbilityModifyStat : AbilityBehavior
{
    readonly AbilityModifyStatDefinition def;
    public AbilityModifyStat(AbilityModifyStatDefinition d) => def = d;
    
    private void ModifyStat(HookType type)
    {
        foreach (StatConfig config in def.configs)
            if (config.hookType.HasFlag(type) && Execution.Handler.TryGetComponent(out StatsHandler stats))
                stats.TryModifyStat(config.statType, config.modifyMax, config.amount);
    }

    public override void OnActivate() => ModifyStat(HookType.OnActivate);
}
