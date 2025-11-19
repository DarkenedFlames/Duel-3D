using UnityEngine;
using System;

[Flags]
public enum Event
{
    None = 0,
    OnTriggerEnter = 1 << 0,
    OnTriggerExit = 2 << 0,
    OnTriggerStay = 3 << 0,
    OnDestroy = 4 << 0,
}

public abstract class Reaction : MonoBehaviour
{
    [Tooltip("The Unity Events that trigger the reaction."), SerializeField]
    protected Event events;
    protected bool ShouldReact(Event e) => events.HasFlag(e);
}