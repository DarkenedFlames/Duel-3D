using UnityEngine;

public enum Event
{
    OnTargetEnter,
    OnTargetExit,
    OnExpire,
    OnPulse,
    OnCollide,
    OnSwing,
    OnApply,
    OnStackLost,
    OnStackGained,
    OnRefresh,
    OnActivate,
    OnEnd
}

[RequireComponent(typeof(LocalEventSource))]
public class LocalEventListener : MonoBehaviour
{
    [SerializeReference] EventReaction reaction;

    protected LocalEventSource Source { get; private set; }

    protected virtual void Awake()
    {
        if (!TryGetComponent(out LocalEventSource source))
        {
            Debug.LogError($"{name} has a LocalEventListener, but no LocalEventSource is present.");
            return;
        }

        Source = source;
        foreach (Event evt in reaction.Events)
            Source.Register(evt, reaction.OnEvent);
    }

    protected virtual void OnDestroy()
    {
        if (Source == null) return;

        foreach (Event evt in reaction.Events)
            Source.Unregister(evt, reaction.OnEvent);
    }
}

