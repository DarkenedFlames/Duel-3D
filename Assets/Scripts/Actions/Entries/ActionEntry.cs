using System;

/// <summary>
/// Base class for pairing actions with their lifecycle hooks.
/// </summary>
[Serializable]
public abstract class ActionEntry
{
    [UnityEngine.SerializeReference]
    public IGameAction Action;
}
