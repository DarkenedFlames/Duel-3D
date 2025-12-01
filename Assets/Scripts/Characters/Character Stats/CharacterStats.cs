using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public List<StatDefinition> InitialStats;

    public Dictionary<Stat, FloatCounter> RegenerationCounters = new();

    public List<Stat> Stats { get; private set; } = new();

    public event Action<Stat> OnStatLearned;

    void Awake()
    {
        foreach (StatDefinition definition in InitialStats)
        {
            if (!TryLearnStat(definition, out Stat _))
                continue;
        }
    }

    public bool TryGetStat(StatDefinition definition, out Stat stat)
    {
        stat = Stats.Find(s => s.Definition == definition);
        return stat != null;
    }

    bool TryLearnStat(StatDefinition definition, out Stat newStat)
    {
        if (TryGetStat(definition, out Stat _))
        {
            newStat = null;
            Debug.LogWarning($"{gameObject.name}'s {nameof(CharacterStats)} tried to learn a duplicate stat from definition {definition.name}!");
        }

        else
        {
            newStat = new(definition);
            Stats.Add(newStat);
            OnStatLearned?.Invoke(newStat);
        }

        return newStat != null;
    }
}