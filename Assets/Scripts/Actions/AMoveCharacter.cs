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
            Debug.LogError("The Target passed to AMoveCharacter is null!");
            return;
        }
        if (context.Source == null)
        {
            Debug.LogError("The Source passed to AMoveCharacter is null!");
            return;
        }
        if (!context.Target.TryGetComponent(out MovementProcessor movement))
        {
            Debug.LogError($"AMoveCharacter expected {context.Target.name} to have a MovementProcessor but it is missing!");
            return;
        }
        if(!context.TryGetSourceTransform(out Transform sourceTransform))
        {
            Debug.LogError("Couldn't find source transform in AMoveCharacter.");
            return;
        }

        Vector3 localDir = direction == Vector3.zero ? Vector3.forward : direction;
        Vector3 pushDirection = sourceTransform.TransformDirection(localDir).normalized;
        movement.ApplyExternalVelocity(pushDirection * forceStrength);
    }
}