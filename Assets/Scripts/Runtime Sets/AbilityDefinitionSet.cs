using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

[CreateAssetMenu]
public class AbilityDefinitionSet : ScriptableObject
{
    public List<AbilityDefinition> definitions = new();

    public AbilityDefinition GetAbilityWeightedByType()
    {
        Dictionary<AbilityType, List<AbilityDefinition>> abilityMap = new()
        {
            { AbilityType.Primary, new() },
            { AbilityType.Secondary, new() },
            { AbilityType.Utility, new() },
            { AbilityType.Special, new() },
        };

        foreach (AbilityDefinition definition in definitions)
            abilityMap[definition.abilityType].Add(definition);

        // Assumes at least one of each ability type exists
        int r1 = (int)Mathf.Round(UnityEngine.Random.value * (abilityMap.Values.Count() - 1));
        AbilityType type = abilityMap.Keys.ToList()[r1];

        int r2 = (int)Mathf.Round(UnityEngine.Random.value * (abilityMap[type].Count() - 1));
        return abilityMap[type][r2];
    }
}