using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Ability Definition", menuName = "Abilities/New Definition")]
public class AbilityDefinition : ScriptableObject
{
    [Header("General")]
    public string abilityName;
    public AbilityType abilityType = AbilityType.Primary;

    [Header("Behaviors")]
    public List<AbilityBehaviorDefinition> behaviors = new();
}