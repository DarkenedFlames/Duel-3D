using System;

/// <summary>
/// Pairs a region lifecycle hook with an action to execute.
/// </summary>
[Serializable]
public class RegionActionEntry : ActionEntry
{
    public RegionHook Hook;
}
