using UnityEngine;

[CreateAssetMenu(menuName = "Duel/Abilities/Behaviors/SpawnsArea")]
public class AbilitySpawnsAreaDefinition : AbilityBehaviorDefinition
{
    [Header("Area configs")]
    public AreaConfig[] configs;

    public override AbilityBehavior CreateRuntimeBehavior() => new AbilitySpawnsArea(this);
    
}

public class AbilitySpawnsArea : AbilityBehavior
{
    readonly AbilitySpawnsAreaDefinition def;
    public AbilitySpawnsArea(AbilitySpawnsAreaDefinition d) => def = d;

    public override void OnActivate()
    {
        Transform casterTransform = Execution.Handler.transform;
        GameObject caster = Execution.Handler.gameObject;

        foreach (AreaConfig config in def.configs)
        {
            if (config.hookType.HasFlag(HookType.OnActivate))
            {
                Vector3 spawnPosition = casterTransform.TransformPoint(config.spawnOffset);
                Quaternion spawnRotation = casterTransform.rotation * Quaternion.Euler(config.localEulerRotation);
                SpawnerController.Instance.SpawnArea(config.areaPrefab, spawnPosition, spawnRotation, caster);                
            }
        }
    }
}
