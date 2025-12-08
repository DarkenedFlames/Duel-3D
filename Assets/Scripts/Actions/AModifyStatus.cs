using UnityEngine;

[System.Serializable]
public class AModifyStatus : ITargetedAction
{
    [Header("Status Configuration")]
    [Tooltip("Status to modify."), SerializeField]
    StatusDefinition statusDefinition;
    
    [Tooltip("Apply status if mode is checked, otherwise remove status."), SerializeField]
    bool mode = true;
    
    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Target), nameof(Execute), nameof(AModifyStatus), context.Source.GameObject);
            return;
        }
        if (statusDefinition == null)
        {
            LogFormatter.LogNullField(nameof(statusDefinition), nameof(AModifyStatus), context.Source.GameObject);
            return;
        }

        if (mode)
            context.Target.CharacterStatuses.AddStatus(statusDefinition);
        else
            context.Target.CharacterStatuses.RemoveStatus(statusDefinition);
    }
}