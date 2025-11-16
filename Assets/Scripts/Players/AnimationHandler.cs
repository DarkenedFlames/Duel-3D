
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AbilityHandler))]
public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private CharacterMovement movement;
    private IInputProvider input;
    private CharacterController controller;
    private AbilityHandler abilities;

    private readonly float damping = 0.1f;

    private void Awake()
    {
        movement = GetComponent<CharacterMovement>();
        input = GetComponent<IInputProvider>();
        controller = GetComponent<CharacterController>();
        abilities = GetComponent<AbilityHandler>();
    }

    private void OnEnable()
    {
        movement.OnJumped += HandleJumped;
        abilities.OnAbilityActivated += HandleAbilityActivated;
    }

    private void OnDisable()
    {
        movement.OnJumped -= HandleJumped;
    }

    void Update()
    {
        HandleMovement();
        animator.SetBool("IsGrounded", controller.isGrounded);
    }

    private void HandleMovement()
    {
        Vector2 animInput = input.MoveInput;
        if (input.SprintPressed)
            animInput.y *= 2f;

        animator.SetFloat("MoveX", animInput.x, damping, Time.deltaTime);
        animator.SetFloat("MoveY", animInput.y, damping, Time.deltaTime);
    }

    private void HandleJumped() => animator.SetTrigger("Jumped");
    
    public void TriggerAttack(string triggerName) => animator.SetTrigger(triggerName);
    public void HandleAbilityActivated(Ability _) => animator.SetTrigger(AbilityAnimationTrigger.Cast.ToString());
}
