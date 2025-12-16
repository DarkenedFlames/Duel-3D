using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Region Definition", menuName = "Definitions/Region")]
public class RegionDefinition : ScriptableObject
{
    [Header("Generic Info")]
    [Tooltip("Name of the region.")]
    public string RegionName = "Untitled Region";

    [Header("Region Settings")]
    [Tooltip("If true, the region's owner can be targeted by the region.")]
    public bool AffectsSource = false;

    [Tooltip("This is the lifetime before expiration (0 for infinite duration)."), Min(0)]
    public float Duration = 0;

    [Tooltip("The number of hits before expiration (0 for no limit)."), Min(0)]
    public int MaxHits = 0;

    [Tooltip("The time in seconds between each pulse (0 for no pulsing)."), Min(0)]
    public float Period = 0;

    [Tooltip("The layers with which the Region may interact.")]
    public LayerMask LayerMask;

    [Header("Actions")]
    [Tooltip("Configure actions to execute at various region lifecycle hooks.")]
    public List<RegionActionEntry> Actions = new();

    public void ExecuteActions(RegionHook hook, ActionContext context) =>
        Actions
            .FindAll(e => e.Hook == hook)
            .ForEach(e => e.Action?.Execute(context));
}
