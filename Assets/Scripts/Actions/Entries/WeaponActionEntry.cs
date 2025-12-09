using System;

/// <summary>
/// Pairs a weapon lifecycle hook with an action to execute.
/// </summary>
[Serializable]
public class WeaponActionEntry : ActionEntry
{
    public WeaponHook Hook;
}
