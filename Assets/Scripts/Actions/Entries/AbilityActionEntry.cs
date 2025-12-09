using System;

/// <summary>
/// Pairs an ability lifecycle hook with an action to execute.
/// </summary>
[Serializable]
public class AbilityActionEntry : ActionEntry
{
    public AbilityHook Hook;
}
