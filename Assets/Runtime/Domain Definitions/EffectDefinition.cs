using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Effect Definition", menuName = "Effects/New Definition")]
public class EffectDefinition : ScriptableObject
{
    public string effectName;

    [Header("Behaviors")]
    public List<EffectBehaviorDefinition> behaviors = new();
}