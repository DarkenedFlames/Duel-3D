using UnityEngine;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CharacterMovement))]
public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private IInputProvider input;
    private CharacterController controller;
    private CharacterMovement movement;

    void Awake()
    {
        input = GetComponent<IInputProvider>();
        controller = GetComponent<CharacterController>();
        movement = GetComponent<CharacterMovement>();
    }

    void Update()
    {
        UpdateMovement(input.MoveInput, input.Sprinting, controller.isGrounded);
    }

    public void UpdateMovement(Vector2 moveInput, bool isSprinting, bool isGrounded)
    {
        Vector2 animInput = moveInput;
        if (isSprinting)
            animInput.y *= 2f;

        animator.SetFloat("MoveX", animInput.x, 0.1f, Time.deltaTime);
        animator.SetFloat("MoveY", animInput.y, 0.1f, Time.deltaTime);
        animator.SetBool("IsGrounded", isGrounded);
    }

    public void TriggerJump() => animator.SetTrigger("JumpTrigger");

    public void TriggerAttack(string triggerName) => animator.SetTrigger(triggerName);

    public void TriggerAbility(string triggerName) => animator.SetTrigger(triggerName);
}
