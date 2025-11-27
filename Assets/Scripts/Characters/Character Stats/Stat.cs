using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public class Stat
{
    public readonly StatDefinition Definition;
    public float BaseValue 
    {
        get => _baseValue;
        set
        {
            if (_baseValue != value)
            {
                _baseValue = value;
                SetDirty();
            }
        }
    }
    private float _baseValue;

    public virtual float Value 
    { 
        get
        {
            if (isDirty || BaseValue != _lastBaseValue)
            {
                _lastBaseValue = BaseValue;
                _value = FinalValue();
                isDirty = false;
            }
            return _value;
        }
    }

    protected bool isDirty = true;
    protected float _value;
    protected float _lastBaseValue  = float.MinValue;

    protected readonly List<StatModifier> modifiers;
    public readonly ReadOnlyCollection<StatModifier> Modifiers;

    public event Action<Stat> OnValueChanged;

    public Stat(StatDefinition definition) : this()
    {
        Definition = definition;
        BaseValue = Definition.defaultValue;
    }

    public Stat()
    {
        modifiers = new();
        Modifiers = modifiers.AsReadOnly();
    }

    protected void SetDirty()
    {
        isDirty = true;
        OnValueChanged?.Invoke(this);
    }

    public virtual void AddModifier(StatModifier modifier)
    {
        SetDirty();
        modifiers.Add(modifier);
        Debug.Log($"Modifier added {modifier.Type} of value {modifier.Value} for stat {Definition.statName}");
    }

    public virtual bool RemoveModifier(StatModifier modifier)
    {
        if (modifiers.Remove(modifier))
        {
            SetDirty();
            Debug.Log($"Modifier removed {modifier.Type} of value {modifier.Value} for stat {Definition.statName}");
            return true;
        }
        return false;
    }

    protected virtual float FinalValue()
    {
        float totalFlat = 0f;
        float totalPercentAdd = 0f;
        float totalPercentMult = 1f;

        foreach (StatModifier mod in modifiers)
        {
            switch (mod.Type)
            {
                case StatModifierType.Flat:        totalFlat += mod.Value; break;
                case StatModifierType.PercentAdd:  totalPercentAdd += mod.Value; break;
                case StatModifierType.PercentMult: totalPercentMult *= mod.Value; break;
            }
        }

        float finalValue = (BaseValue + totalFlat) * (1 + totalPercentAdd) * totalPercentMult;
        return (float)Math.Round(finalValue, 4);
    }

    public virtual bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;

        for (int i = modifiers.Count - 1; i >= 0; i--)
        {
            if (modifiers[i].Source == source)
            {
                didRemove = true;
                RemoveModifier(modifiers[i]);
            }
        }

        return didRemove;
    }
}