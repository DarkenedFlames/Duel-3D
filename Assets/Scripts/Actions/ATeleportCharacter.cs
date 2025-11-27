using UnityEngine;

[System.Serializable]
public class ATeleportCharacter : IGameAction
{
    [Header("Teleportation Configuration")]
    [Tooltip("Local offset to teleport the character."), SerializeField]
    Vector3 localOffset = Vector3.zero;
    
    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            Debug.LogError($"Action {nameof(ATeleportCharacter)} was passed a null parameter: {nameof(context.Target)}!");
            return;
        }
        if (context.Source == null)
        {
            Debug.LogError($"Action {nameof(ATeleportCharacter)} was passed a null parameter: {nameof(context.Source)}!");
            return;
        }
        if(!context.TryGetSourceTransform(out Transform sourceTransform))
        {
            Debug.LogError($"Action {nameof(ATeleportCharacter)} could not find {nameof(sourceTransform)}!");
            return;
        }

        context.Target.transform.position = sourceTransform.TransformPoint(localOffset);
    }
}