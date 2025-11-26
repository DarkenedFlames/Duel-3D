using UnityEngine;

[System.Serializable]
public class AModifyEffect : IGameAction
{
    [Header("Effect Configuration")]
    [Tooltip("Effect to modify."), SerializeField]
    EffectDefinition effectDefinition;
    
    [Tooltip("Apply stacks if mode is checked, otherwise remove stacks."), SerializeField]
    bool mode;

    [Tooltip("The number of stacks to apply or remove."), SerializeField]
    int stacks;
    
    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            Debug.LogError($"Action {nameof(AModifyEffect)} was passed a null parameter: {nameof(context.Target)}!");
            return;
        }
        if (effectDefinition == null)
        {
            Debug.LogError($"Action {nameof(AModifyEffect)} was configured with a null parameter: {nameof(effectDefinition)}!");
            return;
        }
        if (stacks <= 0)
        {
            Debug.LogError($"Action {nameof(AModifyEffect)} was configured with an invalid parameter: {nameof(stacks)} must be positive!");
            return;
        }
        if (!context.Target.TryGetComponent(out CharacterEffects effects))
        {
            Debug.LogError($"Action {nameof(AModifyEffect)} was passed a parameter with a missing component: {nameof(context.Target)} missing {nameof(CharacterEffects)}!");
            return;
        }

        if (mode) 
            effects.AddEffect(effectDefinition, stacks);
        else
            effects.RemoveEffect(effectDefinition, stacks);
    }
}