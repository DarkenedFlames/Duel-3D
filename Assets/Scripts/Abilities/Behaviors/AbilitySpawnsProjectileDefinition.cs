using UnityEngine;

[CreateAssetMenu(menuName = "Duel/Abilities/Behaviors/SpawnsProjectile")]
public class AbilitySpawnsProjectileDefinition : AbilityBehaviorDefinition
{
    [Tooltip("Projectile prefab to spawn. Should have a ProjectileHandler component.")]
    public GameObject projectilePrefab;
    [Tooltip("Local spawn offset from caster transform")]
    public Vector3 spawnOffset = Vector3.zero;
    [Tooltip("Initial rotation relative to caster")]
    public Vector3 localEulerRotation = Vector3.zero;

    public override AbilityBehavior CreateRuntimeBehavior() => new AbilitySpawnsProjectile(this);
    
}

public class AbilitySpawnsProjectile : AbilityBehavior
{
    AbilitySpawnsProjectileDefinition def;

    public AbilitySpawnsProjectile(AbilitySpawnsProjectileDefinition d) => def = d;

    public override void OnActivate()
    {
        var handler = Execution.Handler;
        var caster = handler.gameObject;
        if (def.projectilePrefab == null) return;

        var spawnPos = handler.transform.TransformPoint(def.spawnOffset);
        var rot = handler.transform.rotation * Quaternion.Euler(def.localEulerRotation);

        // You can pass Execution or other context as userContext if the projectile needs to know the source execution.
        SpawnerController.Instance.SpawnProjectile(def.projectilePrefab, spawnPos, rot, caster);
    }

    public override void OnTick(float dt) { /* no tick needed for simple spawn */ }
    public override void OnEnd() { /* nothing */ }
}
