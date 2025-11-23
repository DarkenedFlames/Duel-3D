using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputDriver))]
[RequireComponent(typeof(CharacterStats))]
public class MovementProcessor : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float speedModifier = 0.05f;
    [SerializeField] float sprintModifier = 0.1f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float rotationSpeed = 300f;
    [SerializeField] float gravity = 9.8f;

    float verticalVelocity;
    Vector3 externalVelocity;

    CharacterController controller;
    PlayerInputDriver input;
    CharacterStats stats;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInputDriver>();
        stats = GetComponent<CharacterStats>();

        input.OnJumpInput += OnJumpInput;
    }

    void OnDestroy()
    {
        input.OnJumpInput -= OnJumpInput;
    }

    void Update()
    {
        // Rotate player horizontally
        float yawDelta = input.lookInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, yawDelta);

        // Convert stats into actual speeds
        if (!stats.TryGetStat("Speed", out ClampedStat speed))
        {
            Debug.LogError($"MovementProcessor couldn't find Speed Stat");
        }

        float moveSpeed = speed.Value * speedModifier;
        float sprintSpeed = speed.Value * sprintModifier;

        // Calculate movement direction
        float currentSpeed = input.sprintingInput ? sprintSpeed : moveSpeed;


        Vector3 moveDir = 
            (transform.forward * input.moveInput.y) + 
            (transform.right * input.moveInput.x);

        // Apply gravity
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0f)
                verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime; 
        }

        Vector3 finalVelocity = moveDir.normalized * currentSpeed;
        finalVelocity.y = verticalVelocity;
        finalVelocity += externalVelocity;

        controller.Move(finalVelocity * Time.deltaTime);
        DampenExternalVelocity();
    }

    void OnJumpInput()
    {
        if (!controller.isGrounded)
            return;

        verticalVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity);
    }

    public void ApplyExternalVelocity(Vector3 velocity)
    {
        externalVelocity += velocity;
    }

    public void DampenExternalVelocity()
    {
        externalVelocity = Vector3.Lerp(externalVelocity, Vector3.zero, Time.deltaTime);
    }
}
