using UnityEngine;
using System;

public class NPCInputDriver : MonoBehaviour, IInputDriver
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool SprintingInput { get; private set; }

    public event Action OnJumpInput;
    public event Action<AbilityType> OnAbilityInput;
    public event Action OnWeaponInput;
    public event Action OnMenuInput;

    void Update()
    {
        // All placeholder
        MoveInput = Vector2.zero;
        LookInput = Vector2.zero;
        SprintingInput = false;

        bool jumping = false;
        if (jumping)
            OnJumpInput?.Invoke();
        
        bool ability = false;
        if (ability)
            OnAbilityInput?.Invoke(AbilityType.Primary);

        bool weapon = false;
        if (weapon)
            OnWeaponInput?.Invoke();

        bool menu = false;
        if (menu)
            OnMenuInput?.Invoke();
    }
}
