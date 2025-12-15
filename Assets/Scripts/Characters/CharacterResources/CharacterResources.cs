using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterResources : MonoBehaviour
{
    public List<ResourceDefinition> InitialResources;

    public Dictionary<ResourceType, CharacterResource> Resources { get; private set; } = new();

    bool initialized = false;

    public event Action<CharacterResource> OnResourceValueChanged;
    public event Action<GameObject> OnDeath;

    Character Owner => GetComponent<Character>();

    void Start() => EnsureInitialized();
    public void EnsureInitialized()
    {
        if (initialized) return;

        foreach (ResourceDefinition definition in InitialResources)
            Resources[definition.resourceType] = new(definition, Owner.CharacterStats.GetStat(definition.MaxStat.statType, this));

        initialized = true;
    }

    public CharacterResource GetResource(ResourceType type, object caller = null)
    {
        if (Resources.TryGetValue(type, out CharacterResource resource))
            return resource;
        else
        {
            Debug.LogError($"{caller} could not find resource of type {type} in {gameObject.name}'s {nameof(CharacterResources)}!");
            return null;
        }
    }
    
    void Update()
    {
        foreach (CharacterResource resource in Resources.Values)
        {
            if (resource.TickRegeneration(out float _))
                OnResourceValueChanged?.Invoke(resource);
        }
        TryDie();
    }

    void TryDie()
    {
        CharacterResource health = GetResource(ResourceType.Health, this);
        if (health == null)
        {
            Debug.LogError($"{gameObject.name} has no Health resource!");
            return;
        }

        if (health.Value > 0)
            return;
        
        Debug.Log($"{gameObject.name} died!");
        OnDeath?.Invoke(gameObject);
        Destroy(gameObject);
    }

    public bool ChangeResourceValue(ResourceType type, float delta, out float changed, bool resetRegenerationIfChanged = false)
    {
        CharacterResource resource = GetResource(type, this);

        bool didChange = resource.ChangeValue(delta, out changed);
        if (didChange)
        {
            OnResourceValueChanged?.Invoke(resource);
            if (resetRegenerationIfChanged)
                resource.RegenerationCounter.Reset();
        }
        return didChange;
    }

    public void AddModifierToResource(ResourceType type, ResourceModifierType modType, float value, object source = null) =>
        GetResource(type, this).AddModifier(new(modType, value, source));
    
    public void RemoveSpecificModifierFromResource(ResourceType type, ResourceModifierType modType, float value, object source = null) =>
        GetResource(type, this).RemoveSpecificModifier(modType, value, source);
    
    public void RemoveAllModifiersFromResource(ResourceType type, object source = null) =>
        GetResource(type, this).RemoveAllModifiers(source);
    
    public void RemoveSpecificModifierFromAllResources(ResourceModifierType type, float value, object source = null)
    {
        foreach (var resource in Resources.Values)
            resource.RemoveSpecificModifier(type, value, source);
    }
    
    public void RemoveAllModifiersFromAllResources(object source = null)
    {
        foreach (var resource in Resources.Values)
            resource.RemoveAllModifiers(source);
    }

    public void RemoveModifiers(ResourceType? type = null, ResourceModifierType? modifierType = null, float? modifierValue = null, object source = null)
    {  
        List<ResourceModifier> toRemove = Resources.Values
            .Where(r => type != null && r.Definition.resourceType == type)
            .Select(r => r.Modifiers)
            .SelectMany(modList => modList)
            .Where(m => modifierType != null && m.Type == modifierType)
            .Where(m => modifierValue != null && m.Value == modifierValue)
            .Where(m => source != null && m.Source == source)
            .ToList();

        
        foreach (CharacterResource resource in Resources.Values)
            foreach (ResourceModifier modifier in resource.Modifiers)
                if (toRemove.Contains(modifier))
                    resource.RemoveModifier(modifier);
    }
}