using System;
using UnityEngine;

[Serializable]
public class ProjectileConfig
{
    [Tooltip("When to spawn the projectile.")]
    public HookType hookType;

    [Tooltip("The projectile to spawn.")]
    public GameObject projectilePrefab;

    [Tooltip("Local spawn offset from caster transform.")]
    public Vector3 spawnOffset = Vector3.zero;

    [Tooltip("Initial rotation relative to caster.")]
    public Vector3 localEulerRotation = Vector3.zero;
}
