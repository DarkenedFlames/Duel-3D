[System.Serializable]
public class ACleanseResourceModifiers : IGameAction
{
    // Remove all modifiers on target that come from source
    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Target), nameof(Execute), nameof(ACleanseResourceModifiers), context.Source.GameObject);
            return;
        }
        if (context.Source == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Source), nameof(Execute), nameof(ACleanseResourceModifiers), context.Source.GameObject);
            return;
        }
        context.Target.CharacterResources.Resources.ForEach(s => s.RemoveAllModifiersFromSource(context.Source));
    }
}
