
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AbilityHandler))]
public class AnimationHandler : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] CharacterMovement movement;
    [SerializeField] CharacterController controller;
    [SerializeField] AbilityHandler abilities;
    [SerializeField] ActorWeaponHandler weapons;
    IInputProvider input; // Interface cannot be seralized

    readonly float damping = 0.1f;

    void Awake()
    {
        input = GetComponent<IInputProvider>();
    }

    void OnEnable()
    {
        movement.OnJumped += HandleJumped;
        abilities.OnAbilityActivated += HandleAbilityActivated;
        weapons.OnSwing += HandleSwing;
    }

    void OnDisable()
    {
        movement.OnJumped -= HandleJumped;
    }

    void Update()
    {
        HandleMovement();
        animator.SetBool("IsGrounded", controller.isGrounded);
    }

    void HandleMovement()
    {
        Vector2 animInput = input.MoveInput;
        if (input.SprintPressed)
            animInput.y *= 2f;

        animator.SetFloat("MoveX", animInput.x, damping, Time.deltaTime);
        animator.SetFloat("MoveY", animInput.y, damping, Time.deltaTime);
    }

    void HandleJumped() => animator.SetTrigger("Jumped");
    void HandleAbilityActivated(Ability _) => animator.SetTrigger(AbilityAnimationTrigger.Cast.ToString());
    void HandleSwing(WeaponSwing swingComponent) => animator.SetTrigger(swingComponent.animationTrigger);
}
