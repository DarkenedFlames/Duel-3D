using System.Collections.Generic;
using UnityEngine;

public enum EffectStackingType { Ignore, AddStackAndRefresh, Refresh, ExtendDuration }

public enum ExpiryType { LoseOneStackAndRefresh, LoseAllStacks }

[CreateAssetMenu(fileName = "New Effect Definition", menuName = "Definitions/Effect")]
public class EffectDefinition : ScriptableObject
{
    [Header("Generic Information")]
    [Tooltip("Effect names are currently used for identification.")]
    public string effectName;

    [Tooltip("The icon associated with the effect.")]
    public Sprite icon;

    [Tooltip("Determines the process used for this effect's stacking.")]
    public EffectStackingType EffectStackingType = EffectStackingType.Refresh;

    [Tooltip("Determines the process used for this effect's expiration.")]
    public ExpiryType expiryType = ExpiryType.LoseAllStacks;

    [Tooltip("The maximum number of stacks this effect can have."), Min(1)]
    public int maxStacks = 1;

    [Tooltip("The lifetime of the effect in seconds (active if greater than 0)."), Min(0)]
    public float duration = 5f;

    [Tooltip("The amount of time between pulses (active if greater than 0)."), Min(0)]
    public float period = 1f;

    [Header("Actions")]
    [Tooltip("Configure actions to execute at various effect lifecycle hooks.")]
    public List<EffectActionEntry> Actions = new();

    public void ExecuteActions(EffectHook hook, ActionContext context) =>
        Actions
            .FindAll(e => e.Hook == hook)
            .ForEach(e => e.Action?.Execute(context));
}