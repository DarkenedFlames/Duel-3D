using UnityEngine;

[CreateAssetMenu(menuName = "Duel/Abilities/Behaviors/SpawnsProjectile")]
public class AbilitySpawnsProjectileDefinition : AbilityBehaviorDefinition
{
    [Header("Projectile Configs")]
    public ProjectileConfig[] configs;

    public override AbilityBehavior CreateRuntimeBehavior() => new AbilitySpawnsProjectile(this);
}

public class AbilitySpawnsProjectile : AbilityBehavior
{
    readonly AbilitySpawnsProjectileDefinition def;
    public AbilitySpawnsProjectile(AbilitySpawnsProjectileDefinition d) => def = d;

    public override void OnActivate()
    {
        Transform casterTransform = Execution.Handler.transform;
        GameObject caster = Execution.Handler.gameObject;

        foreach (ProjectileConfig config in def.configs)
        {
            if (config.hookType.HasFlag(HookType.OnActivate))
            {
                Vector3 spawnPosition = casterTransform.TransformPoint(config.spawnOffset);
                Quaternion spawnRotation = casterTransform.rotation * Quaternion.Euler(config.localEulerRotation);
                SpawnerController.Instance.SpawnProjectile(config.projectilePrefab, spawnPosition, spawnRotation, caster);                
            }
        }
    }
}
