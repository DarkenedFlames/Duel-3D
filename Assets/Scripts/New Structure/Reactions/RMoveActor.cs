using UnityEngine;

public class RMoveActor : Reaction
{
    [Header("MoveActor Configuration")]
    [Tooltip("Local direction to move the actor."), SerializeField]
    Vector3 direction = Vector3.zero;

    [Tooltip("Force strength with which the actor is moved."), SerializeField]
    float forceStrength = 5f;
    
    public void MoveActor(GameObject actor)
    {
        if (actor == null) return;

        if (actor.TryGetComponent(out CharacterMovement movement))
        {
            Vector3 pushDirection = transform.TransformDirection(Vector3.forward + direction).normalized;
            movement.ApplyExternalVelocity(pushDirection * forceStrength);
        }
    }
}