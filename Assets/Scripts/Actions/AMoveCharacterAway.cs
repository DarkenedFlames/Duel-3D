using UnityEngine;

[System.Serializable]
public class AMoveCharacterAway : IGameAction
{
    [Tooltip("Force strength with which the actor is moved."), SerializeField, Min(0)]
    float forceStrength = 10f;
    
    [Tooltip("If true, moves the Character away from the source. Otherwise, moves it towards the source."), SerializeField]
    bool moveAway = true;
    
    public void Execute(ActionContext context)
    {;
        if (context.Target == null)
        {
            LogFormatter.LogNullField(nameof(context.Target), nameof(AMoveCharacterAway), context.Source.GameObject);
            return;
        }
        if (context.Source == null)
        {
            LogFormatter.LogNullField(nameof(context.Source), nameof(AMoveCharacterAway), context.Source.GameObject);
            return;
        }

        Vector3 pushDirection = (context.Target.transform.position - context.Source.Transform.position).normalized;
        if (!moveAway)
            pushDirection *= -1f;
        context.Target.CharacterMovement.ApplyExternalVelocity(pushDirection * forceStrength);
    }
}