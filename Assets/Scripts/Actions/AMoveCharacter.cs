using UnityEngine;

public enum MoveCharacterTarget { Owner, Target }
public enum MoveTargetReference { Owner, Target, Source }
public enum MoveReferenceType { TowardsReference, LocalDirection }
public enum MoveCharacterMode { Force, Reposition }

[System.Serializable]
public class AMoveCharacter : ITargetedAction
{
    [Header("Movement Configuration")]
    [Tooltip("The character to be moved."), SerializeField]
    MoveCharacterTarget target = MoveCharacterTarget.Target;

    [Tooltip("Is the character moved relative to themselves or a reference?"), SerializeField]
    MoveReferenceType referenceType = MoveReferenceType.LocalDirection;

    [Tooltip("The reference transform that the character moves relative to."), SerializeField]
    MoveTargetReference reference = MoveTargetReference.Source;

    [Tooltip("The process used to move the character."), SerializeField]
    MoveCharacterMode mode = MoveCharacterMode.Force;

    [Tooltip("Local direction to move the actor."), SerializeField]
    Vector3 direction = Vector3.forward;
    
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

        Character characterToMove = target switch
        {
            MoveCharacterTarget.Owner  => context.Source.Owner,
            MoveCharacterTarget.Target => context.Target,
            _ => null
        };

        Vector3 movementDirection;
        if (referenceType == MoveReferenceType.TowardsReference)
        {
            Transform referenceTransform = reference switch
            {
                MoveTargetReference.Owner  => context.Source.Owner.transform,
                MoveTargetReference.Target => context.Target.transform,
                MoveTargetReference.Source => context.Source.Transform,
                _ => null
            };

            Vector3 worldDirectionOfReference = (referenceTransform.position - characterToMove.transform.position).normalized;
            Quaternion localRotation = Quaternion.LookRotation(worldDirectionOfReference, Vector3.up);
            movementDirection = localRotation * direction;
        }
        else
            movementDirection = characterToMove.transform.TransformDirection(direction);
        
        if (mode == MoveCharacterMode.Force)
            characterToMove.CharacterMovement.ApplyExternalVelocity(movementDirection);
        else
            characterToMove.transform.position += movementDirection; // Could multiply by context.Magnitude if desired
    }
}