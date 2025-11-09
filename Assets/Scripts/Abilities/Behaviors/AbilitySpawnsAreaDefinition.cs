using UnityEngine;

[CreateAssetMenu(menuName = "Duel/Abilities/Behaviors/SpawnsArea")]
public class AbilitySpawnsAreaDefinition : AbilityBehaviorDefinition
{
    [Tooltip("Area prefab to spawn. Should have a Area component.")]
    public GameObject areaPrefab;
    [Tooltip("Local spawn offset from caster transform")]
    public Vector3 spawnOffset = Vector3.zero;
    [Tooltip("Initial rotation relative to caster")]
    public Vector3 localEulerRotation = Vector3.zero;

    public override AbilityBehavior CreateRuntimeBehavior() => new AbilitySpawnsArea(this);
    
}

public class AbilitySpawnsArea : AbilityBehavior
{
    readonly AbilitySpawnsAreaDefinition def;
    public AbilitySpawnsArea(AbilitySpawnsAreaDefinition d) => def = d;

    public override void OnActivate()
    {
        var handler = Execution.Handler;
        var caster = handler.gameObject;
        if (def.areaPrefab == null) return;

        var spawnPos = handler.transform.TransformPoint(def.spawnOffset);
        var rot = handler.transform.rotation * Quaternion.Euler(def.localEulerRotation);

        SpawnerController.Instance.SpawnArea(def.areaPrefab, spawnPos, rot, caster);
    }
}
