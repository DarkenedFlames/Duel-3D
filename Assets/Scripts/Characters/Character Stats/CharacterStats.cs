using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public List<StatDefinition> InitialStats;

    public Dictionary<StatType, Stat> Stats { get; private set; } = new();

    void Awake()
    {
        foreach (StatDefinition definition in InitialStats)
            Stats[definition.statType] = new(definition);
    }

    public Stat GetStat(StatType type, object caller = null)
    {
        if (Stats.TryGetValue(type, out Stat stat))
            return stat;
        else
        {
            Debug.LogError($"{caller} could not find stat of type {type} in {gameObject.name}'s {nameof(CharacterStats)}!");
            return null;
        }
    }
    
    public void AddModifierToStat(StatType type, StatModifierType modType, float value, object source = null) =>
        GetStat(type, this).AddModifier(new(modType, value, source));

    public void RemoveSpecificModifierFromStat(StatType type, StatModifierType modType, float value, object source = null) =>
        GetStat(type, this).RemoveSpecificModifier(modType, value, source);
    
    public void RemoveAllModifiersFromStat(StatType type, object source = null) =>
        GetStat(type, this).RemoveAllModifiers(source);

    public void RemoveSpecificModifierFromAllStats(StatModifierType type, float value, object source = null)
    {
        foreach (var stat in Stats.Values)
            stat.RemoveSpecificModifier(type, value, source);
    }

    public void RemoveAllModifiersFromAllStats(object source = null)
    {
        foreach (var stat in Stats.Values)
            stat.RemoveAllModifiers(source);
    }
}