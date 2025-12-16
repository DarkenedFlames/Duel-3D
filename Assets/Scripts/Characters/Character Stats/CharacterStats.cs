using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] StatDefinitionSet InitialStatSet;

    readonly List<Stat> Stats = new();

    void Awake()
    {
        foreach (StatDefinition definition in InitialStatSet.Definitions)
            Stats.Add(new(definition));
    }

    public Stat GetStat(StatType type) => Stats.Find(s => s.Definition.statType == type);

    public void AddModifiers(StatModifierType modifierType, float modifierValue, StatType? statType = null,  object source = null)
    {
        foreach (Stat stat in Stats)
            if (statType == null || GetStat(statType.Value) == stat)
                stat.AddModifier(new StatModifier(modifierType, modifierValue, source));
    }

    public void RemoveModifiers(StatType? statType = null, StatModifierType? modifierType = null, float? modifierValue = null, object source = null)
    {
        foreach (Stat stat in Stats)
            if (statType == null || GetStat(statType.Value) == stat)
                stat.RemoveModifiers(modifierType, modifierValue, source);
    }
}