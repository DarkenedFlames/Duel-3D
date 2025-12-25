using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] StatDefinitionSet InitialStatSet;

    readonly List<Stat> stats = new();
    public ReadOnlyCollection<Stat> Stats => stats.AsReadOnly();

    bool initialized = false;

    void Awake() => Initialize();
    public void Initialize()
    {
        if (initialized) return;

        foreach (StatDefinition definition in InitialStatSet.Definitions)
            stats.Add(new(definition));
        
        initialized = true;
    }

    public Stat GetStat(StatType type) => stats.Find(s => s.Definition.statType == type);

    public Dictionary<StatDefinition, List<StatModifier>> AddModifiers(
        StatModifierType modifierType,
        float modifierValue,
        StatType? statType = null,
        object source = null
    )
    {
        Dictionary<StatDefinition, List<StatModifier>> added = new();
        foreach (Stat stat in stats)
            if (statType == null || GetStat(statType.Value) == stat)
            {
                StatModifier newModifier = new(modifierType, modifierValue, source);
                added[stat.Definition] = new(){ newModifier };
                stat.AddModifier(newModifier);
            }
        return added;
    }

    public Dictionary<StatDefinition, List<StatModifier>> RemoveModifiers(
        StatType? statType = null,
        StatModifierType? modifierType = null,
        float? modifierValue = null,
        object source = null
    )
    {
        Dictionary<StatDefinition, List<StatModifier>> removed = new();
        foreach (Stat stat in stats)
            if (statType == null || GetStat(statType.Value) == stat)
                removed.Add(
                    stat.Definition,
                    stat.RemoveModifiers(modifierType, modifierValue, source)
                 );
        return removed;
    }
}