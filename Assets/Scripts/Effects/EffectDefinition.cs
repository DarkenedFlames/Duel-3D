using UnityEngine;
using System.Collections.Generic;



[CreateAssetMenu(fileName = "New Effect Definition", menuName = "Duel/Effects/Definition")]
public class EffectDefinition : ScriptableObject
{
    public string effectName;
    public StackingType stackingType = StackingType.Refresh;
    public ExpiryType expiryType = ExpiryType.LoseAllStacks;
    public int maxStacks = 1;
    public float duration = 5f;

    [Header("Behaviors")]
    public List<EffectBehaviorDefinition> behaviors = new();
}
