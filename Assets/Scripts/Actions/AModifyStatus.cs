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
        if (context.Target == null)
        {
            Debug.LogError($"Action {nameof(AModifyStatus)} was passed a null parameter: {nameof(context.Target)}!");
            return;
        }
        if (!context.Target.TryGetComponent(out CharacterStatuses statuses))
        {
            Debug.LogError($"Action {nameof(AModifyStatus)} was passed a parameter with a missing component: {nameof(CharacterStatuses)}!");
            return;
        }

        if (mode)
            statuses.AddStatus(statusDefinition);
        else
            statuses.RemoveStatus(statusDefinition);
    }
}