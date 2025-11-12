using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Spawns Area Behavior", menuName = "Duel/Projectiles/Behaviors/SpawnsArea")]
public class ProjectileSpawnsAreaDefinition : ProjectileBehaviorDefinition
{
    [Header("Area configs")]
    public AreaConfig[] configs;

    public override ProjectileBehavior CreateRuntimeBehavior(Projectile projectile) => new ProjectileSpawnsArea(projectile, this);
    
}

public class ProjectileSpawnsArea : ProjectileBehavior
{
    private new ProjectileSpawnsAreaDefinition Definition => (ProjectileSpawnsAreaDefinition)base.Definition;
    public ProjectileSpawnsArea(Projectile projectile, ProjectileSpawnsAreaDefinition definition) : base(projectile, definition) { }

    private void SpawnArea(HookType type)
    {
        foreach (AreaConfig config in Definition.configs)
        {
            if (config.hookType.HasFlag(type))
            {
                Transform projectileTransform = Projectile.transform;
                Vector3 spawnPosition = projectileTransform.TransformPoint(config.spawnOffset);
                Quaternion spawnRotation = projectileTransform.rotation * Quaternion.Euler(config.localEulerRotation);
                SpawnerController.Instance.SpawnArea(config.areaPrefab, spawnPosition, spawnRotation, Projectile.SourceActor);
            }
        }
    }

    public override void OnCollide(GameObject target) => SpawnArea(HookType.OnCollide);
}
