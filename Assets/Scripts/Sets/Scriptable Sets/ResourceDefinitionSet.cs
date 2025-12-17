using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Resource Definition Set", menuName = "Scriptable Definition Sets/Resource")]

public class ResourceDefinitionSet : ScriptableObject
{
    public List<ResourceDefinition> Definitions = new();

    public bool TryGetRandomResourceDefinition(out ResourceDefinition resourceDefinition)
    {
        resourceDefinition = Definitions.Count == 0 ? null : Definitions[Random.Range(0, Definitions.Count)];
        return resourceDefinition != null;
    }
}