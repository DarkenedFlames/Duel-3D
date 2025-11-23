using UnityEngine;
using System;

public class PlayerInputDriver : MonoBehaviour
{
    public Vector2 moveInput;
    public Vector2 lookInput;
    public bool sprintingInput;
    public event Action OnJumpInput;
    public event Action<AbilityType> OnAbilityInput;
    public event Action OnWeaponInput;

    void Update()
    {
        moveInput = new(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        lookInput = new(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );

        sprintingInput = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Space)) OnJumpInput?.Invoke();

        if (Input.GetKeyDown(KeyCode.Alpha1)) OnAbilityInput?.Invoke(AbilityType.Primary);
        if (Input.GetKeyDown(KeyCode.Alpha2)) OnAbilityInput?.Invoke(AbilityType.Secondary);
        if (Input.GetKeyDown(KeyCode.Alpha3)) OnAbilityInput?.Invoke(AbilityType.Utility);
        if (Input.GetKeyDown(KeyCode.Alpha4)) OnAbilityInput?.Invoke(AbilityType.Special);

        if (Input.GetMouseButtonDown(0)) OnWeaponInput?.Invoke();
    }
}
