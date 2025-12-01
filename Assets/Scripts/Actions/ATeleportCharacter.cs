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
            LogFormatter.LogNullField(nameof(context.Target), nameof(ATeleportCharacter), context.Source.GameObject);
            return;
        }
        if (context.Source == null)
        {
            LogFormatter.LogNullField(nameof(context.Source), nameof(ATeleportCharacter), context.Source.GameObject);
            return;
        }

        context.Target.transform.position = context.Source.Transform.TransformPoint(localOffset);
    }
}