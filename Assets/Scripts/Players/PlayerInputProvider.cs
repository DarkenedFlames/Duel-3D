using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerInputProvider : MonoBehaviour, IInputProvider
{
    [Header("Input Actions")]
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference lookAction;
    [SerializeField] InputActionReference jumpAction;
    [SerializeField] InputActionReference sprintAction;
    [SerializeField] InputActionReference attackAction;
    [SerializeField] InputActionReference[] abilityActions;

    public Vector2 MoveInput => moveAction.action.ReadValue<Vector2>();
    public Vector2 LookInput => lookAction.action.ReadValue<Vector2>();
    public bool JumpPressed => jumpAction.action.triggered;
    public bool SprintPressed => sprintAction.action.IsPressed() && MoveInput.y > 0;
    public bool AttackPressed => attackAction.action.triggered;
    public bool[] AbilityPressed => abilityActions.Select(a => a.action.triggered).ToArray();

    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        jumpAction.action.Enable();
        sprintAction.action.Enable();
        attackAction.action.Enable();
        foreach (InputActionReference action in abilityActions) action.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        jumpAction.action.Disable();
        sprintAction.action.Disable();
        attackAction.action.Disable();
        foreach (InputActionReference action in abilityActions) action.action.Disable();
    }
}
