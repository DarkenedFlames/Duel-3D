using UnityEngine;
public class AIInputProvider : MonoBehaviour, IInputProvider
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool Sprinting { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool[] AbilityPressed { get; private set; } = new bool[4];

    public void ThinkAndDecide()
    {
        // Example: chase player
        MoveInput = new Vector2(0, 1);
        LookInput = Vector2.zero;
        AttackPressed = true;
    }
}
