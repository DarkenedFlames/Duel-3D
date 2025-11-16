
using UnityEngine;
public class AIInputProvider : MonoBehaviour, IInputProvider
{
    public Vector2 MoveInput { get; private set; } = new(0, 0);
    public Vector2 LookInput { get; private set; } = new(0, 0);
    public bool JumpPressed { get; private set; } = false;
    public bool SprintPressed { get; private set; } = false;
    public bool AttackPressed { get; private set; } = false;
    public bool[] AbilityPressed { get; private set; } = new bool[4];
}
