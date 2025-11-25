using UnityEngine;
using System;

public class PlayerInputDriver : MonoBehaviour, IInputDriver
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool SprintingInput { get; private set; }

    public event Action OnJumpInput;
    public event Action<AbilityType> OnAbilityInput;
    public event Action OnWeaponInput;

    void Update()
    {
        MoveInput = new(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        LookInput = new(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );

        SprintingInput = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Space)) OnJumpInput?.Invoke();

        if (Input.GetKeyDown(KeyCode.Alpha1)) OnAbilityInput?.Invoke(AbilityType.Primary);
        if (Input.GetKeyDown(KeyCode.Alpha2)) OnAbilityInput?.Invoke(AbilityType.Secondary);
        if (Input.GetKeyDown(KeyCode.Alpha3)) OnAbilityInput?.Invoke(AbilityType.Utility);
        if (Input.GetKeyDown(KeyCode.Alpha4)) OnAbilityInput?.Invoke(AbilityType.Special);

        if (Input.GetMouseButtonDown(0)) OnWeaponInput?.Invoke();
    }
}
