using UnityEngine;

public interface IInputProvider
{
    Vector2 MoveInput { get; }
    Vector2 LookInput { get; }
    bool JumpPressed { get; }
    bool SprintPressed { get; }
    bool AttackPressed { get; }
    bool[] AbilityPressed { get; }

}
