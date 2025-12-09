using UnityEngine;

public enum MoveTargetReference { Owner, Target, Source }
public enum MoveReferenceType { TowardsReference, LocalDirection }
public enum MoveCharacterMode { Force, Reposition }

[System.Serializable]
public class AMoveCharacter : IGameAction
{
    [Header("Movement Configuration")]
    [Tooltip("The character to be moved: Owner (caster/summoner) or Target (hit character)."), SerializeField]
    ActionTargetMode characterToMove = ActionTargetMode.Target;

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
        Character target = characterToMove switch
        {
            ActionTargetMode.Owner => context.Source.Owner,
            ActionTargetMode.Target => context.Target,
            _ => null,
        };

        if (target == null)
        {
            Debug.LogWarning($"{nameof(AMoveCharacter)}: {characterToMove} is null. Action skipped.");
            return;
        }

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

            if (referenceTransform == null)
            {
                Debug.LogWarning($"{nameof(AMoveCharacter)}: Reference transform is null. Action skipped.");
                return;
            }

            Vector3 worldDirectionOfReference = (referenceTransform.position - target.transform.position).normalized;
            Quaternion localRotation = Quaternion.LookRotation(worldDirectionOfReference, Vector3.up);
            movementDirection = localRotation * direction;
        }
        else
            movementDirection = target.transform.TransformDirection(direction);
        
        if (mode == MoveCharacterMode.Force)
            target.CharacterMovement.ApplyExternalVelocity(movementDirection);
        else
            target.transform.position += movementDirection; // Could multiply by context.Magnitude if desired
    }
}