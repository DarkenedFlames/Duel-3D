using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
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

    // This is being called every frame probably because regen even at max. Fix.
    public bool ChangeValue(float delta, out float changed)
    {
        changed = 0;
        if (Mathf.Approximately(0, delta)) return false;

        GetModifierTotals(out float increase, out float decrease);

        if (delta > 0)
            delta *= increase;
        else
            delta *= decrease;
        
        if (Mathf.Approximately(0, delta)) return false;
    
        _value = Mathf.Clamp(_value + delta, 0, MaxStat.Value);
        changed = delta;
        OnValueChanged?.Invoke(this);
        return true;
    }

    public bool TickRegeneration(out float changed)
    {
        changed = 0;
        RegenerationCounter.Decrease(Time.deltaTime);
        if (!RegenerationCounter.Expired) return false;

        float regenAmount = MaxStat.Value * Definition.RegenerationPercentage * Time.deltaTime;
        
        return ChangeValue(regenAmount, out changed);
    }

    public virtual void AddModifier(ResourceModifier mod) => modifiers.Add(mod);
    public virtual void RemoveModifier(ResourceModifier mod) => modifiers.Remove(mod);

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

    public virtual void RemoveAllModifiers(object source = null)
    {
        if (source == null)
            modifiers.Clear();
        else
        {
            for (int i = modifiers.Count - 1; i >= 0; i--)
                if (modifiers[i].Source == source)
                    RemoveModifier(modifiers[i]);
        }
    }

    public virtual void RemoveSpecificModifier(ResourceModifierType type, float value, object source = null)
    {
        for (int i = modifiers.Count - 1; i >= 0; i--)
            if (modifiers[i].Type == type && Mathf.Approximately(modifiers[i].Value, value) && (source == null || modifiers[i].Source == source))
                RemoveModifier(modifiers[i]);
    }
}