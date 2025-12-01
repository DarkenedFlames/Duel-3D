using UnityEngine;

public interface IGameAction
{
    void Execute(ActionContext context);
}

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