using UnityEngine;
using System.Collections.Generic;

public enum AbilityAnimationTrigger { None, Cast, Channel }
public enum AbilityType { Primary, Secondary, Utility, Special }

[CreateAssetMenu(fileName = "New Ability Definition", menuName = "Definitions/Ability")]
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
    public ResourceType ExpendedResource = ResourceType.Mana;

    [Tooltip("The amount of the expended resource spent per cast."), Min(0)]
    public float resourceCost = 0f;

    [Tooltip("The cooldown per cast in seconds."), Min(0)]
    public float cooldown = 0f;

    [Header("Actions")]
    [Tooltip("Configure actions to execute when ability is cast.")]
    public List<AbilityActionEntry> Actions = new();

    public void ExecuteActions(AbilityHook hook, ActionContext context) =>
        Actions
            .FindAll(e => e.Hook == hook)
            .ForEach(e => e.Action?.Execute(context));
}
