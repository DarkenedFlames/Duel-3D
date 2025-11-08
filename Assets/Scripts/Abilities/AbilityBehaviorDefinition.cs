using UnityEngine;

[System.Flags]
public enum AbilityTag { None }

// Base ScriptableObject for behavior definitions.
// Create concrete derived definitions (SpawnProjectileBehaviorDefinition, ApplyAOEDamageBehaviorDefinition, etc.)
public abstract class AbilityBehaviorDefinition : ScriptableObject
{
    [Tooltip("Lower numbers run earlier.")]
    public int defaultPriority = 100;

    // Optionally require/forbid tags on the caster container
    public AbilityTag requiredTags = AbilityTag.None;
    public AbilityTag forbiddenTags = AbilityTag.None;

    // Create the runtime behavior instance (hooked to the execution container)
    public abstract AbilityBehavior CreateRuntimeBehavior();
}
