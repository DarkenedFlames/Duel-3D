using UnityEngine;

[System.Serializable]
public class AMoveCharacter : IGameAction
{
    [Header("Movement Configuration")]
    [Tooltip("Local direction to move the actor."), SerializeField]
    Vector3 direction = new(0, 0, 1);

    [Tooltip("Force strength with which the actor is moved."), SerializeField, Min(0)]
    float forceStrength = 5f;
    
    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            LogFormatter.LogNullField(nameof(context.Target), nameof(AMoveCharacter), context.Source.GameObject);
            return;
        }
        if (context.Source == null)
        {
            LogFormatter.LogNullField(nameof(context.Source), nameof(AMoveCharacter), context.Source.GameObject);
            return;
        }

        Vector3 pushDirection = context.Source.Transform.TransformDirection(direction).normalized;
        context.Target.CharacterMovement.ApplyExternalVelocity(pushDirection * forceStrength);
    }
}