using UnityEngine;

[System.Serializable]
public class AMoveCharacter : IGameAction
{
    [Header("Movement Configuration")]
    [Tooltip("Local direction to move the actor."), SerializeField]
    Vector3 direction = Vector3.zero;

    [Tooltip("Force strength with which the actor is moved."), SerializeField]
    float forceStrength = 5f;
    
    public void Execute(GameObject source, GameObject target)
    {
        if (target == null || source == null) return;

        if (target.TryGetComponent(out MovementProcessor movement))
        {
            Vector3 pushDirection = source.transform.TransformDirection(Vector3.forward + direction).normalized;
            movement.ApplyExternalVelocity(pushDirection * forceStrength);
        }
    }
}