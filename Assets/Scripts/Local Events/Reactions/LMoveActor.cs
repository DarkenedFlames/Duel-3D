using UnityEngine;

[System.Serializable]
public class LMoveActor : EventReaction
{
    [Header("MoveActor Configuration")]
    [Tooltip("Local direction to move the actor."), SerializeField]
    Vector3 direction = Vector3.zero;

    [Tooltip("Force strength with which the actor is moved."), SerializeField]
    float forceStrength = 5f;
    
    public override void OnEvent(EventContext context)
    {
        if (context.defender == null) return;
        if (context.source == null) return;

        if (context.defender.TryGetComponent(out CharacterMovement movement))
        {
            Vector3 pushDirection = context.source.transform.TransformDirection(Vector3.forward + direction).normalized;
            movement.ApplyExternalVelocity(pushDirection * forceStrength);
        }
    }
}