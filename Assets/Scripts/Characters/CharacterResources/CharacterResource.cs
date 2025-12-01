using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class CharacterResource
{
    public ResourceDefinition Definition;
    public Stat MaxStat;

    public float Value => _value;
    private float _value;

    private readonly List<ResourceModifier> modifiers;
    public ReadOnlyCollection<ResourceModifier> Modifiers;

    public FloatCounter RegenerationCounter;

    public event Action<CharacterResource> OnValueChanged;

    public CharacterResource(ResourceDefinition definition, Stat maxStat)
    {
        Definition = definition;
        MaxStat = maxStat;
        _value = maxStat.Value;

        RegenerationCounter = new(0, 0, Definition.RegenerationCooldown, true, true);
        modifiers = new();
        Modifiers = modifiers.AsReadOnly();
    }

    public virtual void AddModifier(ResourceModifier mod) => modifiers.Add(mod);
    public virtual void RemoveModifier(ResourceModifier mod) => modifiers.Remove(mod);

    public bool ChangeValue(float delta, out float changed)
    {
        changed = 0;
        if (Mathf.Approximately(0, delta)) return false;

        GetModifierTotals(out float increase, out float decrease);

        if (delta > 0)
            delta *= increase;
        else
            delta *= decrease;
        
        delta = Mathf.Clamp(delta, -1f * _value, MaxStat.Value - _value);

        if (Mathf.Approximately(0, delta)) return false;
    
        changed = delta;
        _value += delta;
        Debug.Log($"[{Definition.ResourceName}] changed to {_value}!");
        OnValueChanged?.Invoke(this);
        return true;
    }

    public void TickRegeneration()
    {
        if (RegenerationCounter == null) return;

        RegenerationCounter.Decrease(Time.deltaTime);
        if (!RegenerationCounter.Expired) return;

        float regenAmount = MaxStat.Value * Definition.RegenerationPercentage;
        
        ChangeValue(regenAmount, out float _);
    }

    public void GetModifierTotals(out float increase, out float decrease)
    {
        increase = 1f;
        decrease = 1f;

        foreach (ResourceModifier modifier in modifiers)
        {
            switch (modifier.Type)
            {
                case ResourceModifierType.Increase: increase *= modifier.Value; break;
                case ResourceModifierType.Decrease: decrease *= modifier.Value; break;
                default: break;
            }
        }
    }

    public virtual void RemoveAllModifiersFromSource(object source)
    {
        for (int i = modifiers.Count - 1; i >= 0; i--)
            if (modifiers[i].Source == source)
                RemoveModifier(modifiers[i]);
    }
}