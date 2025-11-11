using System;
using UnityEngine;

[Serializable]
public class AreaConfig
{
    [Tooltip("When this applies")]
    public HookType hookType;

    [Tooltip("Area to spawn")]
    public GameObject areaPrefab;

    [Tooltip("Local spawn offset from caster transform")]
    public Vector3 spawnOffset = Vector3.zero;

    [Tooltip("Initial rotation relative to caster")]
    public Vector3 localEulerRotation = Vector3.zero;
}
