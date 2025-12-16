using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System.Linq;

public class Stat
{
    public readonly StatDefinition Definition;

    protected float _baseValue;
    public virtual float BaseValue => _baseValue;

    protected float _value;
    public virtual float Value => _value;

    protected readonly List<StatModifier> modifiers;
    public readonly ReadOnlyCollection<StatModifier> Modifiers;

    public event Action<Stat> OnValueChanged;

    public Stat(StatDefinition definition)
    {
        Definition = definition;
        _baseValue = definition.defaultValue;
        modifiers = new();
        Modifiers = modifiers.AsReadOnly();
        RecalculateValue();
    }

    protected virtual void RecalculateValue()
    {
        _value = FinalValue();
        OnValueChanged?.Invoke(this);
    }

    protected virtual float FinalValue()
    {
        GetModifierTotals(out float flat, out float percentAdd, out float percentMult);
        return (BaseValue + flat) * (1 + percentAdd) * percentMult;
    }

    protected virtual void GetModifierTotals(out float flat, out float percentAdd, out float percentMult)
    {
        flat = 0f;
        percentAdd = 0f;
        percentMult = 1f;

        foreach (var mod in modifiers)
        {
            switch (mod.Type)
            {
                case StatModifierType.Flat: flat += mod.Value; break;
                case StatModifierType.PercentAdd: percentAdd += mod.Value; break;
                case StatModifierType.PercentMult: percentMult *= mod.Value; break;
            }
        }
    }

    public virtual void AddModifier(StatModifier mod)
    {
        modifiers.Add(mod);
        RecalculateValue();
    }

    public void RemoveModifiers(StatModifierType? modifierType = null, float? modifierValue = null, object source = null)
    {  
        List<StatModifier> toRemove = modifiers
            .Where(m => modifierType != null && m.Type == modifierType)
            .Where(m => modifierValue != null && Mathf.Approximately(m.Value, modifierValue.Value))
            .Where(m => source != null && m.Source == source)
            .ToList();

        for (int i = modifiers.Count - 1; i >= 0; i--)
            if (toRemove.Contains(modifiers[i]) && modifiers.Remove(modifiers[i]))
                    RecalculateValue();
    }
}
