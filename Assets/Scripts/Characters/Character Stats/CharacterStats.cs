using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] public List<StatDefinition> InitialStats;
    public List<ClampedStat> Stats { get; private set; } = new();
    public event Action<Stat> OnStatLearned;
    public event Action<GameObject> OnDeath;
    public event Action<GameObject> OnTakeDamage;

    void Awake()
    {
        foreach (StatDefinition definition in InitialStats)
            LearnStat(definition);
    }

    public bool TryGetStat(string statName, out ClampedStat stat)
    {
        stat = Stats.FirstOrDefault(s => s.Definition.statName == statName);
        return stat != null;
    }

    void LearnStat(StatDefinition definition)
    {
        if (!TryGetStat(definition.statName, out ClampedStat _))
        {
            ClampedStat newStat = new(definition, new Stat(definition));
            Stats.Add(newStat);
            OnStatLearned?.Invoke(newStat);
        }
    }
    
    void Update()
    {
        TryDie();
    }

    public void TakeDamage(float amount)
    {
        if (TryGetStat("Health", out ClampedStat health))
        {
            health.BaseValue -= amount;
            Debug.Log($"{gameObject.name} took {amount} damage!");
            OnTakeDamage?.Invoke(gameObject);  
        }
    }

    void TryDie()
    {
        if (TryGetStat("Health", out ClampedStat health) && health.Value <= 0)
        {
            Debug.Log($"{gameObject.name} died!");
            OnDeath?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }
}