using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Weapon Definition", menuName = "Weapons/New Definition")]
public class WeaponDefinition : ScriptableObject
{
    [Header("General")]
    public GameObject weaponPrefab;
    public string weaponName;

    [Header("Behaviors")]
    public List<WeaponBehaviorDefinition> behaviors;
}