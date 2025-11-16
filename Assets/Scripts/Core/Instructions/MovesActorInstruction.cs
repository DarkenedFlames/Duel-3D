using System;
using UnityEngine;

[Serializable]
public class MovesActorInstruction : IInstruction
{
    [Tooltip("Local direction to move the actor.")]
    public Vector3 direction = Vector3.zero;

    [Tooltip("Force strength with which the actor is moved.")]
    public float forceStrength = 5f;
    
    public void Execute(IInstructionContext context)
    {
        if (context.Actor.TryGetComponent(out CharacterMovement movement))
        {
            Vector3 pushDirection = context.Domain.transform.TransformDirection(Vector3.forward + direction).normalized;
            movement.ApplyExternalVelocity(pushDirection * forceStrength);
        }
    }
}