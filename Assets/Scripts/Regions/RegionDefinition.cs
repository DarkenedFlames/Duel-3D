using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class RegionDefinition : ScriptableObject
{
    [Tooltip("Name of the region.")]
    public string RegionName;

    [Tooltip("When actions are execute.")]
    public RegionExecutionType RegionExecutionType;

    [Tooltip("When the region expires.")]
    public RegionExpiryType RegionExpiryType;

    [Tooltip("If Region Expiry Type includes Duration, this is the lifetime before expiration.")]
    public float Duration;

    [Tooltip("If Region Execution Type includes Periodic, this is the time between pulses.")]
    public float Period;

    [Tooltip("If Region Expiry Type includes Hit Limit, this is the lifetime before expiration.")]
    public int MaxHits;

    [Tooltip("The movement configuration for the Region.")]
    public IRegionMover Mover;

    [Tooltip("The layers with which the Region may interact.")]
    public LayerMask LayerMask;

    [Tooltip("The actions to execute.")]
    public List<IGameAction> Actions = new();
}
