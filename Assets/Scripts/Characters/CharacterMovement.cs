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
    [SerializeField] float gravity = 15f;
    [SerializeField] float slideGravity = 5f; // Force applied when sliding down slopes

    [Header("Fall Damage Settings")]
    [SerializeField] float fallDamageThreshold = 7.5f;
    [SerializeField, Range(0,1)] float fallDamagePercentMaxHealthPerMeter = .01f;
    [SerializeField] GameObject fallParticlePrefab;

    [Header("Debug")]
    [SerializeField] bool showSlopeDebug = false;

    float verticalVelocity;
    Vector3 externalVelocity;
    float externalVelocityDamping = 1f;

    bool wasGrounded;

    Character owner;
    CharacterController controller;
    IInputDriver input;

    void Awake()
    {
        owner = GetComponent<Character>();

        if (!TryGetComponent(out IInputDriver inputDriver))
            Debug.LogError($"{name}'s {nameof(CharacterMovement)} expected a component but it was missing: {nameof(IInputDriver)} missing!");
        else input = inputDriver;

        controller = GetComponent<CharacterController>();
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
        Stat speed = owner.CharacterStats.GetStat(StatType.MovementSpeed);

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
            if (!wasGrounded && verticalVelocity < -fallDamageThreshold)
            {
                CharacterStats stats = owner.CharacterStats;
                float maxHealthValue = stats.GetStat(StatType.MaxHealth).Value;
                owner.CharacterResources.ChangeResourceValue(
                    ResourceType.Health,
                    -1f * (Mathf.Abs(verticalVelocity) - fallDamageThreshold) * maxHealthValue * fallDamagePercentMaxHealthPerMeter,
                    out _,
                    true
                );
                SpawnFallParticles();
            }

            if (verticalVelocity < 0f)
                verticalVelocity = -1f;
        }
        else
            verticalVelocity -= gravity * Time.deltaTime;

        wasGrounded = controller.isGrounded; 
        
        Vector3 finalVelocity = moveDir.normalized * currentSpeed;
        finalVelocity.y = verticalVelocity;
        finalVelocity += externalVelocity;

        // Apply slope sliding if on too-steep slope
        finalVelocity += GetSlopeSlideVelocity();

        controller.Move(finalVelocity * Time.deltaTime);
        DampenExternalVelocity();

        // Debug slope info
        if (showSlopeDebug)
            DebugSlopeInfo();
    }

    Vector3 GetSlopeSlideVelocity()
    {
        // Check if we're standing on a surface that's too steep
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2f + 0.5f))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            
            // If slope is steeper than the slope limit, apply sliding
            if (slopeAngle > controller.slopeLimit)
            {
                // Project down the slope
                Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal);
                return slideDirection * slideGravity;
            }
        }
        
        return Vector3.zero;
    }

    void DebugSlopeInfo()
    {
        // Raycast down to detect slope angle
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2f + 0.5f))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            Debug.Log($"Slope Angle: {slopeAngle:F1}° | Slope Limit: {controller.slopeLimit}° | " +
                      $"Step Offset: {controller.stepOffset} | Skin Width: {controller.skinWidth} | " +
                      $"IsGrounded: {controller.isGrounded}");
            
            // Visualize the slope normal
            Debug.DrawRay(hit.point, hit.normal * 2f, Color.green);
            Debug.DrawRay(hit.point, Vector3.up * 2f, Color.blue);
        }
    }

    void OnJumpInput()
    {
        if (!controller.isGrounded)
            return;

        verticalVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity);
    }

    public void ApplyExternalVelocity(Vector3 velocity, float damping = 1f)
    {
        externalVelocity += velocity;
        externalVelocityDamping = damping;
    }

    public void DampenExternalVelocity()
    {
        externalVelocity = Vector3.Lerp(externalVelocity, Vector3.zero, externalVelocityDamping * Time.deltaTime);
    }

    public void SpawnFallParticles()
    {
        if (fallParticlePrefab != null)
            Instantiate(fallParticlePrefab, transform.position, Quaternion.identity);
    }
}
