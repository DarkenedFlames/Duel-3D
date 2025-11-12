using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(AnimationHandler), typeof(StatsHandler))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speedStatConversionModifier = 0.05f;
    [SerializeField] private float sprintModifier = 2f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float externalDamping = 5f;

    private CharacterController controller;
    private AnimationHandler animationHandler;
    private StatsHandler statsHandler;

    private Vector3 velocity;
    private bool isGrounded;
    private Vector3 externalForce;


    public Vector2 Look { get; private set; }

    IInputProvider input;

    private void Awake()
    {
        input = GetComponent<IInputProvider>();
        controller = GetComponent<CharacterController>();
        animationHandler = GetComponent<AnimationHandler>();
        statsHandler = GetComponent<StatsHandler>();
    }

    void Update()
    {
        ProcessMovement(input.MoveInput, input.Sprinting);
        if (input.JumpPressed) TryJump();
    }


    public void ProcessMovement(Vector2 moveInput, bool sprintIsPressed)
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float speedStat = statsHandler.GetStat(StatType.Speed, getMax: false);

        float walkSpeed = speedStat * speedStatConversionModifier;
        float speed = sprintIsPressed ? walkSpeed * sprintModifier : walkSpeed;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        Vector3 totalMove = (speed * move) + externalForce;

        // Apply both the input and the force
        controller.Move(totalMove * Time.deltaTime);

        // Apply gravity as usual
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Dampen external forces gradually
        externalForce = Vector3.Lerp(externalForce, Vector3.zero, Time.deltaTime * externalDamping);
    }

    public void ProcessLook(Vector2 lookInput) => Look = lookInput;
    public void ApplyExternalForce(Vector3 force) => externalForce += force;

    public void TryJump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animationHandler.TriggerJump();
        }
    }    
}
