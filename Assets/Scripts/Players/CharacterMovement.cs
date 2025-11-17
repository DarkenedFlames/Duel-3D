using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(AnimationHandler), typeof(StatsHandler))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Actor Components")]
    [SerializeField] CharacterController controller;
    [SerializeField] StatsHandler statsHandler;
    IInputProvider input; // Interface cannot be serialized

    [Header("Movement Settings")]
    [SerializeField] float speedStatConversionModifier = 0.05f;
    [SerializeField] float sprintModifier = 2f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float externalDamping = 5f;

    Vector3 velocity;
    Vector3 verticalVelocity;
    Vector3 horizontalVelocity;
    Vector3 externalVelocity;
    bool IsGrounded => controller.isGrounded;

    public Action OnJumped;



    public void ApplyExternalVelocity(Vector3 amount) => externalVelocity += amount;

    void Awake()
    {
        input = GetComponent<IInputProvider>();
    }

    void Update()
    {
        UpdateGroundState();
        HandleJumping();
        HandleHorizontalMovement();
        ApplyGravity();
        ApplyExternalVelocity();

        velocity = horizontalVelocity + verticalVelocity + externalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    void UpdateGroundState()
    {
        if (IsGrounded && verticalVelocity.y < 0) verticalVelocity.y = -2f;
    }

    void HandleHorizontalMovement()
    {
        float baseSpeed = statsHandler.GetStat(StatType.Speed, getMax: false) * speedStatConversionModifier;

        float finalSpeed = input.SprintPressed ? baseSpeed * sprintModifier : baseSpeed;

        Vector3 moveDir = transform.right * input.MoveInput.x + transform.forward * input.MoveInput.y;

        horizontalVelocity = moveDir.normalized * finalSpeed;
    }

    void HandleJumping()
    {
        if (IsGrounded && input.JumpPressed)
        {
            OnJumped?.Invoke();
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void ApplyGravity() => verticalVelocity.y += gravity * Time.deltaTime;
    void ApplyExternalVelocity() => externalVelocity = Vector3.Lerp(externalVelocity, Vector3.zero, Time.deltaTime * externalDamping);
    
}
