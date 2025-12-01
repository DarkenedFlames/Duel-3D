using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

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
        Debug.Log($"[{Definition.statName}] changed to {_value}!");
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
        Debug.Log($"[{Definition.statName}] Modifier Added: {mod.Value} {mod.Type}");
    }

    public virtual bool RemoveModifier(StatModifier mod)
    {
        bool removed = modifiers.Remove(mod);
        if (removed)
        {
            RecalculateValue();
            Debug.Log($"[{Definition.statName}] Modifier Added: {mod.Value} {mod.Type}");
        }
        return removed;
    }

    public void ApplyFlat(float amount, object source = null) =>
        AddModifier(new StatModifier(StatModifierType.Flat, amount, source));

    public void ApplyPercent(float percent, object source = null) =>
        AddModifier(new StatModifier(StatModifierType.PercentAdd, percent, source));
    
    public void ApplyMultiplier(float multiplier, object source = null) =>
        AddModifier(new StatModifier(StatModifierType.PercentMult, multiplier, source));

    public virtual void RemoveAllModifiersFromSource(object source)
    {
        for (int i = modifiers.Count - 1; i >= 0; i--)
            if (modifiers[i].Source == source)
                RemoveModifier(modifiers[i]);
    }
}
