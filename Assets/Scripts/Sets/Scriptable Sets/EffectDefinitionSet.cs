using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Effect Definition Set", menuName = "Scriptable Definition Sets/Effect")]

public class EffectDefinitionSet : ScriptableObject
{
    public List<EffectDefinition> Definitions = new();
}