using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(AbilityHandler))]
public class InputHandler : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference sprintAction;
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private InputActionReference castPrimaryAction;
    [SerializeField] private InputActionReference castSecondaryAction;
    [SerializeField] private InputActionReference castUtilityAction;
    [SerializeField] private InputActionReference castSpecialAction;


    private PlayerMovement movementHandler;
    private WeaponHandler weaponHandler;
    private AbilityHandler abilityHandler;

    private void Awake()
    {
        movementHandler = GetComponent<PlayerMovement>();
        weaponHandler = GetComponentInChildren<WeaponHandler>();
        abilityHandler = GetComponent<AbilityHandler>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        jumpAction.action.Enable();
        sprintAction.action.Enable();
        attackAction.action.Enable();
        castPrimaryAction.action.Enable();
        castSecondaryAction.action.Enable();
        castUtilityAction.action.Enable();
        castSpecialAction.action.Enable();

        attackAction.action.started += OnAttackStarted;
        jumpAction.action.started += OnJumpStarted;

        castPrimaryAction.action.started += OnPrimaryCastStarted;
        castSecondaryAction.action.started += OnSecondaryCastStarted;
        castUtilityAction.action.started += OnUtilityCastStarted;
        castSpecialAction.action.started += OnSpecialCastStarted;
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        jumpAction.action.Disable();
        sprintAction.action.Disable();
        attackAction.action.Disable();
        castPrimaryAction.action.Disable();
        castSecondaryAction.action.Disable();
        castUtilityAction.action.Disable();
        castSpecialAction.action.Disable();

        attackAction.action.started -= OnAttackStarted;
        jumpAction.action.started -= OnJumpStarted;

        castPrimaryAction.action.started -= OnPrimaryCastStarted;
        castSecondaryAction.action.started -= OnSecondaryCastStarted;
        castUtilityAction.action.started -= OnUtilityCastStarted;
        castSpecialAction.action.started -= OnSpecialCastStarted;
    }

    private void Update()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();
        bool wantsToSprint = sprintAction.action.IsPressed() && moveInput.y > 0f;

        movementHandler.ProcessMovement(moveInput, wantsToSprint);
        movementHandler.ProcessLook(lookInput);
    }

    public void SetWeaponHandler(WeaponHandler handler) => weaponHandler = handler;

    private void OnJumpStarted(InputAction.CallbackContext ctx) => movementHandler.TryJump();

    private void OnAttackStarted(InputAction.CallbackContext ctx)
    {
        if (weaponHandler != null) weaponHandler.Attack();
    }

    private void OnPrimaryCastStarted(InputAction.CallbackContext ctx) => abilityHandler.Cast(AbilityType.Primary);
    private void OnSecondaryCastStarted(InputAction.CallbackContext ctx) => abilityHandler.Cast(AbilityType.Secondary);
    private void OnUtilityCastStarted(InputAction.CallbackContext ctx) => abilityHandler.Cast(AbilityType.Utility);
    private void OnSpecialCastStarted(InputAction.CallbackContext ctx) => abilityHandler.Cast(AbilityType.Special);

}
