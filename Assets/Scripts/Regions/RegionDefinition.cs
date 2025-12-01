using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
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
