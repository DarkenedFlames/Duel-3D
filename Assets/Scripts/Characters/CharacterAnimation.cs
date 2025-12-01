using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CharacterAbilities))]
[RequireComponent(typeof(CharacterWeapons))]
public class CharacterAnimation : MonoBehaviour
{
    Animator animator;
    CharacterController controller;
    IInputDriver input;
    CharacterAbilities abilities;
    CharacterWeapons weapons;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        input = GetComponent<IInputDriver>();
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
        if (input == null) Debug.Log("Input null");
        if (animator == null) Debug.Log("Animator null");
        if (controller == null) Debug.Log("Controller null");

        Vector2 animInput = input.MoveInput;
        if (input.SprintingInput) animInput.y *= 2f;
        animator.SetFloat("MoveX", animInput.x, 0.1f, Time.deltaTime);
        animator.SetFloat("MoveY", animInput.y, 0.1f, Time.deltaTime);
        animator.SetBool("IsGrounded", controller.isGrounded);
    }

    void HandleJumped() => animator.SetTrigger("Jumped");

    void HandleAbilityActivated(Ability _) => animator.SetTrigger(AbilityAnimationTrigger.Cast.ToString());
    void HandleWeaponUsed(Weapon weaponComponent) => animator.SetTrigger(weaponComponent.Definition.AnimationTrigger);
}
