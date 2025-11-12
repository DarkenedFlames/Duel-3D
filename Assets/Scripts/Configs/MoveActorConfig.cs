using System;
using UnityEngine;

[Serializable]
public class MoveActorConfig
{
    [Tooltip("When to move the actor.")]
    public HookType hookType;

    [Tooltip("Direction to move the actor, relative to the projectile's transform.")]
    public Vector3 direction = Vector3.zero;

    [Tooltip("Force strength with which the actor is moved.")]
    public float forceStrength = 5f;
}
