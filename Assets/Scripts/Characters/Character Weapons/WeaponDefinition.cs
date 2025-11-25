using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponDefinition : ScriptableObject
{
    [Header("Generic Information")]
    [Tooltip("The name associated with the weapon.")]
    public string WeaponName;

    [Tooltip("The icon associated with the weapon.")]
    public Sprite Icon;

    [Header("Weapon Settings")]
    [Tooltip("The stat that is expended when the weapon is used.")]
    public StatDefinition ExpendedStat;

    [Tooltip("The amount of the expended stat that is expended when the weapon is used.")]
    public float StatCost;

    [Tooltip("The cooldown between uses in seconds.")]
    public float CooldownTime;

    [Tooltip("The amount of time it takes to use the weapon.")]
    public float UseTime;

    [Tooltip("The character animation trigger for the weapon.")]
    public string AnimationTrigger = "AttackTrigger";

    [Tooltip("The layers this weapon can hit.")]
    public LayerMask layerMask;

    [Tooltip("The actions to be executed on the targets the weapon hits."), SerializeReference]
    public List<IGameAction> OnHitActions;
}