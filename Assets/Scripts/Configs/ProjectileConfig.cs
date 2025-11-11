using System;
using UnityEngine;

[Serializable]
public class ProjectileConfig
{
    [Tooltip("When this applies")]
    public HookType hookType;

    [Tooltip("Projectile to spawn")]
    public GameObject projectilePrefab;

    [Tooltip("Local spawn offset from caster transform")]
    public Vector3 spawnOffset = Vector3.zero;

    [Tooltip("Initial rotation relative to caster")]
    public Vector3 localEulerRotation = Vector3.zero;
}
