using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(AnimationHandler), typeof(StatsHandler))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speedStatConversionModifier = 0.05f;
    [SerializeField] private float sprintModifier = 2f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private AnimationHandler animationHandler;
    private StatsHandler statsHandler;

    private Vector3 velocity;
    private bool isGrounded;

    public Vector2 Look { get; private set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animationHandler = GetComponent<AnimationHandler>();
        statsHandler = GetComponent<StatsHandler>();
    }

    public void ProcessMovement(Vector2 moveInput, bool isSprinting)
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (!statsHandler.TryGetStat(StatType.Speed, out float speedStat))
            Debug.LogError($"No StatType.Speed found for PlayerMovement on {gameObject.name}");

        float walkSpeed = speedStat * speedStatConversionModifier;
        float speed = isSprinting ? walkSpeed * sprintModifier : walkSpeed;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(speed * Time.deltaTime * move);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        animationHandler.UpdateMovement(moveInput, isSprinting, isGrounded);
    }

    public void ProcessLook(Vector2 lookInput) => Look = lookInput;
    

    public void TryJump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animationHandler.TriggerJump();
        }
    }
}
