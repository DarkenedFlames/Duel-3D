using UnityEngine;

[System.Serializable]
public class AMoveCharacterAway : IGameAction
{
    [Tooltip("Force strength with which the actor is moved."), SerializeField]
    float forceStrength = 10f;
    [Tooltip("If true, moves the Character away from the source. Otherwise, moves it towards the source."), SerializeField]
    bool moveAway = true;
    
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

        Vector3 pushDirection = (context.Target.transform.position - sourceTransform.position).normalized;
        if (!moveAway)
            pushDirection *= -1f;
        movement.ApplyExternalVelocity(pushDirection * forceStrength);
    }
}