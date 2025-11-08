using UnityEngine;

/// <summary>
/// Defines what runtime EffectBehavior to create.
/// </summary>
public abstract class EffectBehaviorDefinition : ScriptableObject
{
    public abstract EffectBehavior CreateRuntimeBehavior(Effect owner);
}
