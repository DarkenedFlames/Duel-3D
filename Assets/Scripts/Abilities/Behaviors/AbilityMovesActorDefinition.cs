using UnityEngine;

[CreateAssetMenu(fileName = "New Ability Moves Actor Behavior", menuName = "Duel/Abilities/Behaviors/MovesActor")]
public class AbilityMovesActorDefinition : AbilityBehaviorDefinition
{
    [Header("Movement Configs")]
    public MoveActorConfig[] configs;

    public override AbilityBehavior CreateRuntimeBehavior() => new AbilityMovesActor(this);
}

public class AbilityMovesActor : AbilityBehavior
{
    readonly AbilityMovesActorDefinition def;
    public AbilityMovesActor(AbilityMovesActorDefinition d) => def = d;
    
    private void MovesActor(HookType type)
    {
        foreach (MoveActorConfig config in def.configs)
            if (config.hookType.HasFlag(type) && Execution.Handler.TryGetComponent(out PlayerMovement movement))
            {
                Vector3 pushDirection = (Execution.Handler.transform.forward + config.direction).normalized;
                movement.ApplyExternalForce(pushDirection * config.forceStrength);
            }
    }

    public override void OnActivate() => MovesActor(HookType.OnActivate);
}
