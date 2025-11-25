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

    void Update()
    {
        MoveInput = Vector2.zero;
        LookInput = Vector2.zero;
        SprintingInput = false;
    }
}
