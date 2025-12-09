using UnityEngine;

[System.Serializable]
public class AModifyStatus : IGameAction
{
    [Header("Target Configuration")]
    [Tooltip("Who to modify: Owner (caster/summoner) or Target (hit character)."), SerializeField]
    ActionTargetMode targetMode = ActionTargetMode.Target;

    [Header("Status Configuration")]
    [Tooltip("Status to modify."), SerializeField]
    StatusDefinition statusDefinition;
    
    [Tooltip("Apply status if mode is checked, otherwise remove status."), SerializeField]
    bool mode = true;
    
    public void Execute(ActionContext context)
    {
        Character target = targetMode switch
        {
            ActionTargetMode.Owner => context.Source.Owner,
            ActionTargetMode.Target => context.Target,
            _ => null,
        };

        if (target == null)
        {
            Debug.LogWarning($"{nameof(AModifyStatus)}: {targetMode} is null. Action skipped.");
            return;
        }

        if (statusDefinition == null)
        {
            LogFormatter.LogNullField(nameof(statusDefinition), nameof(AModifyStatus), context.Source.GameObject);
            return;
        }

        if (mode)
            target.CharacterStatuses.AddStatus(statusDefinition);
        else
            target.CharacterStatuses.RemoveStatus(statusDefinition);
    }
}