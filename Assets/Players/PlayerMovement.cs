using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AnimationHandler))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private AnimationHandler animationHandler;

    private Vector3 velocity;
    private bool isGrounded;

    public Vector2 Look { get; private set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animationHandler = GetComponent<AnimationHandler>();
    }

    public void ProcessMovement(Vector2 moveInput, bool wantsToSprint)
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float speed = wantsToSprint ? sprintSpeed : walkSpeed;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(speed * Time.deltaTime * move);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Notify animation handler
        animationHandler.UpdateMovement(moveInput, wantsToSprint, isGrounded);
    }

    public void ProcessLook(Vector2 lookInput)
    {
        Look = lookInput;
    }

    public void TryJump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animationHandler.TriggerJump();
        }
    }
}
