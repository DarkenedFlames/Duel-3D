using System;

/// <summary>
/// Pairs an effect lifecycle hook with an action to execute.
/// </summary>
[Serializable]
public class EffectActionEntry : ActionEntry
{
    public EffectHook Hook;
}
