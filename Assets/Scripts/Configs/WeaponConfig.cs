using System;
using UnityEngine;

[Serializable]
public class WeaponConfig
{
    [Tooltip("When to give the weapon.")]
    public HookType hookType;

    [Tooltip("The weapon to give.")]
    public GameObject weaponPrefab;
}
