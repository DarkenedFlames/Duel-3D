using UnityEngine;
using HBM.Scriptable;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputDriver))]
[RequireComponent(typeof(CharacterAbilities))]
[RequireComponent(typeof(CharacterWeapons))]
public class PlayerAnimationProcessor : MonoBehaviour
{
    Animator animator;
    CharacterController controller;
    PlayerInputDriver input;
    CharacterAbilities abilities;
    CharacterWeapons weapons;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInputDriver>();
        abilities = GetComponent<CharacterAbilities>();
        weapons = GetComponent<CharacterWeapons>();
    }

    void OnEnable()
    {   
        input.OnJumpInput += HandleJumped;
        abilities.OnAbilityActivated += HandleAbilityActivated;
        weapons.OnWeaponUsed += HandleWeaponUsed;
    }

    void OnDisable()
    {
        input.OnJumpInput -= HandleJumped;
        abilities.OnAbilityActivated -= HandleAbilityActivated;
        weapons.OnWeaponUsed += HandleWeaponUsed;
    }

    void Update()
    {
        Vector2 animInput = input.moveInput;
        if (input.sprintingInput) animInput.y *= 2f;
        animator.SetFloat("MoveX", animInput.x, 0.1f, Time.deltaTime);
        animator.SetFloat("MoveY", animInput.y, 0.1f, Time.deltaTime);
        animator.SetBool("IsGrounded", controller.isGrounded);
    }

    void HandleJumped() => animator.SetTrigger("Jumped");

    void HandleAbilityActivated(Ability _) => animator.SetTrigger(AbilityAnimationTrigger.Cast.ToString());
    void HandleWeaponUsed(Weapon weaponComponent) => animator.SetTrigger(weaponComponent.Definition.AnimationTrigger);
}
