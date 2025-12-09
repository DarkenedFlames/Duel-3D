using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CharacterStats))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] StatDefinition speedStat;
    [SerializeField] float speedModifier = 0.05f;
    [SerializeField] float sprintModifier = 0.1f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float rotationSpeed = 300f;
    [SerializeField] float gravity = 9.8f;

    float verticalVelocity;
    Vector3 externalVelocity;

    CharacterController controller;
    IInputDriver input;
    CharacterStats stats;

    void Awake()
    {
        if (!TryGetComponent(out IInputDriver inputDriver))
            Debug.LogError($"{name}'s {nameof(CharacterMovement)} expected a component but it was missing: {nameof(IInputDriver)} missing!");
        else input = inputDriver;

        controller = GetComponent<CharacterController>();
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
        float yawDelta = input.LookInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, yawDelta);

        // Convert stats into actual speeds
        Stat speed = stats.GetStat(StatType.MovementSpeed, this);

        float moveSpeed = speed.Value * speedModifier;
        float sprintSpeed = speed.Value * sprintModifier;

        // Calculate movement direction
        float currentSpeed = input.SprintingInput ? sprintSpeed : moveSpeed;


        Vector3 moveDir = 
            (transform.forward * input.MoveInput.y) + 
            (transform.right * input.MoveInput.x);

        // Apply gravity
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0f)
                verticalVelocity = -1f;
        }
        else
            verticalVelocity -= gravity * Time.deltaTime; 
        
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
