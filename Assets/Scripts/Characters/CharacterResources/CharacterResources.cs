using System;
using System.Collections.Generic;
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
        if (GetResource(ResourceType.Health, this).Value > 0)
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
}