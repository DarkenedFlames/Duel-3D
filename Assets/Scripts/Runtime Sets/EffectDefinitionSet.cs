using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class EffectDefinitionSet : ScriptableObject
{
    public List<EffectDefinition> Definitions = new();
}