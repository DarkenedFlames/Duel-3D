using UnityEngine;

public class FiresProjectile : AbilityBehavior<FiresProjectileDefinition>
{
    public FiresProjectile(FiresProjectileDefinition def, Ability owner) : base(def, owner) { }
    public override void OnUpdate(float deltaTime) { }

    public override void OnCast()
    {
        if (Definition.projectilePrefab == null)
        {
            Debug.LogWarning($"Ability {Owner.Definition.abilityName} has no projectile prefab assigned!");
            return;
        }

        GameObject caster = Owner.Handler.gameObject;

        Transform casterTransform = caster.transform;

        for (int i = 0; i < Definition.projectileCount; i++)
        {
            // Slight random spread
            Quaternion spreadRot = Quaternion.Euler(
                Random.Range(-Definition.spreadAngle, Definition.spreadAngle),
                Random.Range(-Definition.spreadAngle, Definition.spreadAngle),
                0
            );

            Vector3 spawnPos = casterTransform.position + casterTransform.TransformDirection(Definition.originOffset);
            Quaternion spawnRot = casterTransform.rotation * spreadRot * Quaternion.Euler(Definition.directionOffset);

            SpawnerController.Instance.SpawnProjectile(Definition.projectilePrefab, spawnPos, spawnRot, caster);
        }
    }
}
