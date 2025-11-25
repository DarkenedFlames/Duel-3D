using UnityEngine;

[System.Serializable]
public class ATeleportCharacter : IGameAction
{
    [Header("Teleportation Configuration")]
    [Tooltip("Local offset to teleport the character."), SerializeField]
    Vector3 localOffset = Vector3.zero;
    
    public void Execute(GameObject source, GameObject target)
    {
        if (target == null || source == null) return;
        target.transform.position = source.transform.TransformPoint(localOffset);
    }
}