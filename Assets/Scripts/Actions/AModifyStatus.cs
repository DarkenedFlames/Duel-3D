using UnityEngine;

[System.Serializable]
public class AModifyStatus : IGameAction
{
    [Header("Status Configuration")]
    [Tooltip("Status to modify."), SerializeField]
    StatusDefinition statusDefinition;
    
    [Tooltip("Apply status if mode is checked, otherwise remove status."), SerializeField]
    bool mode;
    
    public void Execute(ActionContext context)
    {
        if (context.Target == null) return;

        // ADD NULL CHECKS

        // Target is guaranteed to have CharacterStatuses because it is a Character
        CharacterStatuses statuses = context.Target.GetComponent<CharacterStatuses>();

        if (mode)
            statuses.AddStatus(statusDefinition);
        else
            statuses.RemoveStatus(statusDefinition);
    }
}