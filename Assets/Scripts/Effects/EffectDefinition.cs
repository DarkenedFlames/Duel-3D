using UnityEngine;
using System.Collections.Generic;

public enum StackingType { None, RefreshDuration, AddStacks, Ignore }
public enum ExpireType { FullRemove, LoseOneStackAndRefresh, LoseOneStackNoRefresh }

[CreateAssetMenu(fileName = "New Effect Definition", menuName = "Duel/Effects/Definition")]
public class EffectDefinition : ScriptableObject
{
    public string effectName;
    public StackingType stackingType = StackingType.RefreshDuration;
    public ExpireType expireType = ExpireType.FullRemove;
    public int maxStacks = 1;
    public float duration = 5f;

    [Header("Behaviors")]
    public List<EffectBehaviorDefinition> behaviors = new();
}
