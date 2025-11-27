using UnityEngine;

[System.Serializable]
public class AMoveCharacter : IGameAction
{
    [Header("Movement Configuration")]
    [Tooltip("Local direction to move the actor."), SerializeField]
    Vector3 direction = Vector3.zero;

    [Tooltip("Force strength with which the actor is moved."), SerializeField]
    float forceStrength = 5f;
    
    public void Execute(ActionContext context)
    {;
        if (context.Target == null)
        {
            Debug.LogError($"Action {nameof(AMoveCharacter)} was passed a null parameter: {nameof(context.Target)}!");
            return;
        }
        if (context.Source == null)
        {
            Debug.LogError($"Action {nameof(AMoveCharacter)} was passed a null parameter: {nameof(context.Source)}!");
            return;
        }
        if (!context.Target.TryGetComponent(out MovementProcessor movement))
        {
            Debug.LogError($"Action {nameof(AMoveCharacter)} was passed a parameter with a missing component: {nameof(MovementProcessor)}!");
            return;
        }
        if (Mathf.Approximately(direction.x, 0) && Mathf.Approximately(direction.y, 0) && Mathf.Approximately(direction.z, 0))
        {
            Debug.LogError($"Action {nameof(AMoveCharacter)} was configured with an invalid parameter: {nameof(direction)} must be a non-zero vector!");
            return;
        }
        if (Mathf.Approximately(forceStrength, 0))
        {
            Debug.LogError($"Action {nameof(AMoveCharacter)} was configured with an invalid parameter: {nameof(forceStrength)} must be non-zero (might be too small)!");
            return;
        }
        if(!context.TryGetSourceTransform(out Transform sourceTransform))
        {
            Debug.LogError($"Action {nameof(AMoveCharacter)} could not find {nameof(sourceTransform)}!");
            return;
        }

        Vector3 pushDirection = sourceTransform.TransformDirection(direction).normalized;
        movement.ApplyExternalVelocity(pushDirection * forceStrength);
    }
}