using UnityEngine;

public interface IGameAction
{
    void Execute(ActionContext context);
}

/// <summary>
/// Actions that require a target Character (e.g., damage, healing, status effects).
/// These actions should only be used with lifecycle hooks that provide targets.
/// </summary>
public interface ITargetedAction : IGameAction { }

/// <summary>
/// Actions that only require a source (e.g., spawning objects, particles).
/// These actions can be used with any lifecycle hook.
/// </summary>
public interface ISourceAction : IGameAction { }

public class ActionContext
{
    public IActionSource Source;
    public Character Target;
    public float Magnitude = 1f;
}

public interface IActionSource
{
    Character Owner { get; set; }    // Who owns this gameplay object?
    Transform Transform { get; }     // Its world-space transform
    GameObject GameObject { get; }   // Underlying GameObject (when applicable)
}