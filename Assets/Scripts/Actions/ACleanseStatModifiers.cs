[System.Serializable]
public class ACleanseStatModifiers : IGameAction
{
    // Remove all modifiers on target that come from source
    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Target), nameof(Execute), nameof(ACleanseStatModifiers), context.Source.GameObject);
            return;
        }
        if (context.Source == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Source), nameof(Execute), nameof(ACleanseStatModifiers), context.Source.GameObject);
            return;
        }
        context.Target.CharacterStats.Stats.ForEach(s => s.RemoveAllModifiersFromSource(context.Source));
    }
}
