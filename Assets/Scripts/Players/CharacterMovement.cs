using System;
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
    private IInputProvider input;

    private Vector3 velocity;
    private Vector3 verticalVelocity;
    private Vector3 horizontalVelocity;
    private Vector3 externalVelocity;
    private bool isGrounded => controller.isGrounded;

    public Action OnJumped;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animationHandler = GetComponent<AnimationHandler>();
        statsHandler = GetComponent<StatsHandler>();

        input = GetComponent<IInputProvider>();
        if (input == null)
            Debug.LogError($"No IInputProvider found on {name}");
    }

    private void Update()
    {
        UpdateGroundState();
        HandleJumping();
        HandleHorizontalMovement();
        ApplyGravity();
        ApplyExternalVelocity();

        velocity = horizontalVelocity + verticalVelocity + externalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    private void UpdateGroundState()
    {
        if (isGrounded && verticalVelocity.y < 0) verticalVelocity.y = -2f;
    }

    private void HandleHorizontalMovement()
    {
        float baseSpeed = statsHandler.GetStat(StatType.Speed, getMax: false) * speedStatConversionModifier;

        float finalSpeed = input.SprintPressed ? baseSpeed * sprintModifier : baseSpeed;

        Vector3 moveDir = transform.right * input.MoveInput.x + transform.forward * input.MoveInput.y;

        horizontalVelocity = moveDir.normalized * finalSpeed;
    }

    private void HandleJumping()
    {
        if (isGrounded && input.JumpPressed)
        {
            OnJumped?.Invoke();
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void ApplyGravity() => verticalVelocity.y += gravity * Time.deltaTime;
    public void ApplyExternalVelocity(Vector3 amount) => externalVelocity += amount;
    private void ApplyExternalVelocity() => externalVelocity = Vector3.Lerp(externalVelocity, Vector3.zero, Time.deltaTime * externalDamping);
    
}
