using System;
using System.Collections.Generic;
using UnityEngine;


public enum EffectStackingType { Ignore, AddStackAndRefresh, Refresh, ExtendDuration }

public enum ExpiryType { LoseOneStackAndRefresh, LoseAllStacks }

[CreateAssetMenu]
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

    [Tooltip("If true, the actions performed by this effect will scale with the number of stacks it has (Does not apply to OnStackGained/Lost Actions).")]
    public bool ScalesWithStacks = true;

    [Tooltip("The maximum number of stacks this effect can have."), Min(1)]
    public int maxStacks = 1;

    [Tooltip("The lifetime of the effect in seconds (active if greater than 0)."), Min(0)]
    public float duration = 5f;

    [Tooltip("The amount of time between pulses (active if greater than 0)."), Min(0)]
    public float period = 1f;


    [Tooltip("The actions that will be executed on the afflicted target upon application."), SerializeReference]
    public List<IGameAction> OnApplyActions;

    [Tooltip("The actions that will be executed on the afflicted target upon pulsing."), SerializeReference]
    public List<IGameAction> OnPulseActions;

    [Tooltip("The actions that will be executed on the afflicted target upon expiration."), SerializeReference]
    public List<IGameAction> OnExpireActions;

    [Tooltip("The actions that will be executed on the afflicted target upon reaching maximum stacks."), SerializeReference]
    public List<IGameAction> OnMaxStackReachedActions;

    [Tooltip("The actions that will be executed on the afflicted target upon refreshing duration."), SerializeReference]
    public List<IGameAction> OnRefreshedActions;

    [Tooltip("The actions that will be executed on the afflicted target upon gaining a stack."), SerializeReference]
    public List<IGameAction> OnStackGainedActions;

    [Tooltip("The actions that will be executed on the afflicted target upon losing a stack."), SerializeReference]
    public List<IGameAction> OnStackLostActions;

    [Tooltip("The actions that will be executed on the afflicted target upon extending duration."), SerializeReference]
    public List<IGameAction> OnExtendedActions;

}