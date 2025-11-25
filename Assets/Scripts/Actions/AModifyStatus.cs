using UnityEngine;

[System.Serializable]
public class AModifyStatus : IGameAction
{
    [Header("Status Configuration")]
    [Tooltip("Status to modify."), SerializeField]
    StatusDefinition statusDefinition;
    
    [Tooltip("Apply status if mode is checked, otherwise remove status."), SerializeField]
    bool mode;
    
    public void Execute(GameObject source, GameObject target)
    {
        if (target == null) return;

        if (target.TryGetComponent(out CharacterStatuses statuses))
            if (mode)
                statuses.AddStatus(statusDefinition);
            else
                statuses.RemoveStatus(statusDefinition);
    }
}