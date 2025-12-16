using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Stat Definition Set", menuName = "Scriptable Definition Sets/Stat")]

public class StatDefinitionSet : ScriptableObject
{
    public List<StatDefinition> Definitions = new();

    public bool TryGetRandomStatDefinition(out StatDefinition statDefinition)
    {
        statDefinition = Definitions.Count == 0 ? null : Definitions[Random.Range(0, Definitions.Count)];
        return statDefinition != null;
    }
}