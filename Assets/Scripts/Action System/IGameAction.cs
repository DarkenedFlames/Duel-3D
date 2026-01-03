using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Base interface for all game actions. Actions execute gameplay logic based on a context.
/// </summary>
public interface IGameAction
{
    void Execute(ActionContext context);
}

/// <summary>
/// Specifies which character an action should target.
/// </summary>
public enum ActionTargetMode
{
    /// <summary>Target the character who owns the source (caster/summoner).</summary>
    Owner,
    
    /// <summary>Target the character provided in the context (hit character/entered region).</summary>
    Target
}

/// <summary>
/// Standard action context with a source and optional target.
/// </summary>
public class ActionContext
{
    public IActionSource Source;
    public Character Target;
    public float Magnitude = 1f;
}

/// <summary>
/// Represents any gameplay object that can execute actions.
/// </summary>
public interface IActionSource
{
    /// <summary>
    /// The character at the root of the ownership chain.
    /// For abilities: the caster. For regions/projectiles: the original summoner.
    /// </summary>
    Character Owner { get; set; }
    
    /// <summary>
    /// The world-space transform of this action source.
    /// </summary>
    Transform Transform { get; }
    
    /// <summary>
    /// The underlying GameObject (when applicable).
    /// </summary>
    GameObject GameObject { get; }

    float Magnitude { get; set; }
}

// ============================================================================
// Hook Enums - Define lifecycle events for each action source type
// ============================================================================

public enum RegionHook
{
    OnSpawn,              // Executes once when region spawns (no target)
    OnDestroy,            // Executes once when region destroys (no target)
    OnTargetEnter,        // Executes per target entering region
    OnTargetExit,         // Executes per target exiting region
    OnPulse,              // Executes once per pulse (no target)
    OnSpawnPerTarget,     // Executes per target present at spawn
    OnDestroyPerTarget,   // Executes per target present at destruction
    OnPulsePerTarget      // Executes per target present at pulse
}

public enum AbilityHook
{
    OnCast               // Executes when ability is activated
}

public enum EffectHook
{
    OnApply,             // Executes when effect is applied
    OnRemove,            // Executes when effect expires/is removed
    OnPulse,             // Executes periodically
    OnStackGained,       // Executes per stack gained
    OnStackLost,         // Executes per stack lost
    OnRefreshed,         // Executes when duration refreshes
    OnExtended,          // Executes when duration extends
    OnMaxStackReached    // Executes when reaching max stacks
}

public enum WeaponHook
{
    OnHit                // Executes when weapon hits a target
}