using UnityEngine;
using System;

public interface IInputDriver
{
    Vector2 MoveInput { get; }
    Vector2 LookInput { get; }
    bool SprintingInput { get; }

    event Action OnJumpInput;
    event Action<AbilityType> OnAbilityInput;
    event Action OnWeaponInput;
}
