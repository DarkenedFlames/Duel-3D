using UnityEngine;
using System.Collections.Generic;

public enum AbilityAnimationTrigger { None, Cast, Channel }
public enum AbilityType { Primary, Secondary, Utility, Special }

[CreateAssetMenu]
public class AbilityDefinition : ScriptableObject
{
    [Header("Generic Information")]
    [Tooltip("The name associated with the ability.")]
    public string abilityName;

    [Tooltip("The icon associated with the ability.")]
    public Sprite icon;

    [Tooltip("The type of ability, defining its slot.")]
    public AbilityType abilityType;

    [Tooltip("The character animation trigger to use for this ability.")]
    public AbilityAnimationTrigger castAnimationTrigger = AbilityAnimationTrigger.Cast;

    [Tooltip("The amount of mana spent per cast.")]
    public float manaCost = 0f;

    [Tooltip("The cooldown per cast in seconds.")]
    public float cooldown = 0f;

    [Tooltip("The conditions required for this ability to activate."), SerializeReference]
    public ActivationCondition[] activationConditions;

    [Tooltip("The actions this ability executes upon activation."), SerializeReference]
    public List<IGameAction> actions = new();

}
