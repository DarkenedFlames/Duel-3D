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

    [Tooltip("The resource that this ability expends.")]
    public ResourceDefinition expendedResource;

    [Tooltip("The amount of the expended resource spent per cast."), Min(0)]
    public float resourceCost = 0f;

    [Tooltip("The cooldown per cast in seconds."), Min(0)]
    public float cooldown = 0f;

    [Tooltip("The conditions required for this ability to activate."), SerializeReference]
    public ActivationCondition[] activationConditions;

    [Header("Source Actions")]
    [Tooltip("The actions this ability executes upon activation."), SerializeReference]
    public List<ISourceAction> OnCastSource = new();

    [Header("Targeted Actions")]
    [Tooltip("The actions this ability executes targeting the caster upon activation."), SerializeReference]
    public List<ITargetedAction> OnCastTargeted = new();
}
