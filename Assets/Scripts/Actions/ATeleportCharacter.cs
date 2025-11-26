using UnityEngine;

[System.Serializable]
public class ATeleportCharacter : IGameAction
{
    [Header("Teleportation Configuration")]
    [Tooltip("Local offset to teleport the character."), SerializeField]
    Vector3 localOffset = Vector3.zero;
    
    public void Execute(ActionContext context)
    {
        if (context.Target == null || context.Source == null) return;

        
        // ADD NULL CHECKS

        if(!context.TryGetSourceTransform(out Transform sourceTransform))
            Debug.LogError("Couldn't find source transform in ATeleportCharacter.");

        context.Target.transform.position = sourceTransform.TransformPoint(localOffset);
    }
}