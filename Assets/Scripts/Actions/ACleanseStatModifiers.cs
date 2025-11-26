using UnityEngine;

[System.Serializable]
public class ACleanseStatModifiers : IGameAction
{
    // Remove all modifiers on target that come from source
    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            Debug.LogError($"Action {nameof(ACleanseStatModifiers)} was passed a null parameter: {nameof(context.Target)}!");
            return;
        }
        if (context.Source == null)
        {
            Debug.LogError($"Action {nameof(ACleanseStatModifiers)} was passed a null parameter: {nameof(context.Source)}!");
            return;
        }
        if (!context.Target.TryGetComponent(out CharacterStats stats))
        {
            Debug.LogError($"Action {nameof(ACleanseStatModifiers)} was passed a parameter with a missing component: {nameof(context.Target)} missing {nameof(CharacterStats)}!");
            return;
        }

        stats.Stats.ForEach(s => s.RemoveAllModifiersFromSource(context.Source));
    }
}
