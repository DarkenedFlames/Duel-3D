using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu]
public class AbilityDefinitionSet : ScriptableObject
{
    public List<AbilityDefinition> definitions = new();

    public AbilityDefinition GetAbilityWeightedByType(List<AbilityDefinition> exclusions = null)
    {
        Dictionary<AbilityType, List<AbilityDefinition>> abilityMap = new()
        {
            { AbilityType.Primary, new() },
            { AbilityType.Secondary, new() },
            { AbilityType.Utility, new() },
            { AbilityType.Special, new() },
        };

        foreach (AbilityDefinition definition in definitions)
        {
            if (exclusions != null && exclusions.Contains(definition))
                continue;
                
            abilityMap[definition.abilityType].Add(definition);
        }

        var availableTypes = abilityMap.Where(kvp => kvp.Value.Count > 0).ToList();
        
        if (availableTypes.Count == 0)
            return null;

        int r1 = Random.Range(0, availableTypes.Count);
        AbilityType type = availableTypes[r1].Key;

        int r2 = Random.Range(0, abilityMap[type].Count);
        return abilityMap[type][r2];
    }
}