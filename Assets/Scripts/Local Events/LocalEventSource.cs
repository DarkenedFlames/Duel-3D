using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class LocalEventSource : MonoBehaviour
{
    private readonly Dictionary<Event, Action<EventContext>> _events = new();

    public void Fire(Event evt, EventContext context)
    {
        if (_events.TryGetValue(evt, out var action))
            action?.Invoke(context);
    }

    public void Register(Event evt, Action<EventContext> listener)
    {
        if (!_events.ContainsKey(evt))
            _events[evt] = null;

        _events[evt] += listener;
    }

    public void Unregister(Event evt, Action<EventContext> listener)
    {
        if (_events.ContainsKey(evt))
            _events[evt] -= listener;
    }
}
