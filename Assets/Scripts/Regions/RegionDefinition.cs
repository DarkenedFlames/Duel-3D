using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class RegionDefinition : ScriptableObject
{
    [Header("Generic Info")]
    [Tooltip("Name of the region.")]
    public string RegionName;

    [Header("Region Settings")]
    [Tooltip("If true, the region's source can be targeted by the region.")]
    public bool AffectsSource;

    [Tooltip("When the region expires.")]
    public RegionExpiryType RegionExpiryType;

    [Tooltip("If Region Expiry Type includes Duration, this is the lifetime before expiration.")]
    public float Duration;

    [Tooltip("If Region Expiry Type includes Hit Limit, this is the lifetime before expiration.")]
    public int MaxHits;

    [Tooltip("The time in seconds between each pulse.")]
    public float Period;

    [Tooltip("The layers with which the Region may interact.")]
    public LayerMask LayerMask;

    [Header("Actions")]
    [Tooltip("The actions to execute on all valid targets in the region upon instantiation."), SerializeReference]
    public List<IGameAction> OnSpawnActions = new();

    [Tooltip("The actions to execute on all valid targets in the region upon destruction."), SerializeReference]
    public List<IGameAction> OnDestroyActions = new();

    [Tooltip("The actions to execute on all valid targets in the region periodically."), SerializeReference]
    public List<IGameAction> OnPulseActions = new();

    [Tooltip("The actions to execute on each valid target that enters the region."), SerializeReference]
    public List<IGameAction> OnEnterActions = new();

    [Tooltip("The actions to execute on each valid target that exits the region."), SerializeReference]
    public List<IGameAction> OnExitActions = new();
}
