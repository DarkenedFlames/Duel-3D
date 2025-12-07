using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterResources : MonoBehaviour
{
    public List<ResourceDefinition> InitialResources;
    public ResourceDefinition healthDefinition; // Must be present in InitialResources

    public List<CharacterResource> Resources { get; private set; } = new();

    bool _isInitialized = false;
    public bool IsInitialized => _isInitialized;

    public event Action<CharacterResource> OnResourceLearned;
    public event Action<CharacterResource> OnResourceValueChanged;
    public event Action<GameObject> OnDeath;

    Character Owner => GetComponent<Character>();

    void Awake()
    {
        if (!InitialResources.Contains(healthDefinition) || healthDefinition == null)
            Debug.LogError($"{gameObject.name}'s {nameof(CharacterResources)} was configured with an invalid parameter: {nameof(InitialResources)} must reference {nameof(healthDefinition)} but it was missing or null!");
    }

    void Start() => EnsureInitialized();
    public void EnsureInitialized()
    {
        if (_isInitialized) return;

        foreach (ResourceDefinition definition in InitialResources)
            AddResource(definition);
        
        _isInitialized = true;
    }

    public bool TryGetResource(ResourceDefinition definition, out CharacterResource resource)
    {
        resource = Resources.Find(r => r.Definition == definition);
        return resource != null;
    }

    void AddResource(ResourceDefinition definition)
    {
        if (TryGetResource(definition, out CharacterResource _))
            return;
        else
        {
            if (!Owner.CharacterStats.TryGetStat(definition.MaxStat, out Stat maxStat))
            {
                Debug.LogError($"{gameObject.name}'s {nameof(CharacterResources)} could not find MaxStat for resource definition {definition.name}!");
                return;
            }

            CharacterResource newResource = new(definition, maxStat);
            Resources.Add(newResource);
            OnResourceLearned?.Invoke(newResource);
        }
    }
    
    void Update()
    {
        foreach (CharacterResource resource in Resources)
        {
            if (resource.TickRegeneration(out float _))
                OnResourceValueChanged?.Invoke(resource);
        }
        TryDie();
    }

    void TryDie()
    {
        if (!TryGetResource(healthDefinition, out CharacterResource health) || health.Value > 0)
            return;
        
        Debug.Log($"{gameObject.name} died!");
        OnDeath?.Invoke(gameObject);
        Destroy(gameObject);
    }

    public bool ChangeResourceValue(ResourceDefinition definition, float delta, out float changed, bool resetRegenerationIfChanged = false)
    {
        changed = 0;
        if (!TryGetResource(definition, out CharacterResource resource))
            return false;

        bool didChange = resource.ChangeValue(delta, out changed);
        if (didChange)
        {
            OnResourceValueChanged?.Invoke(resource);
            if (resetRegenerationIfChanged)
                resource.RegenerationCounter.Reset();
        }
        return didChange;
    }

    public void AddModifierToResource(ResourceDefinition definition, ResourceModifierType type, float value, object source = null)
    {
        if (!TryGetResource(definition, out CharacterResource resource))
            return;

        resource.AddModifier(new(type, value, source));
    }
    public void RemoveSpecificModifierFromResource(ResourceDefinition definition, ResourceModifierType type, float value, object source = null)
    {
        if (!TryGetResource(definition, out CharacterResource resource))
            return;

        resource.RemoveSpecificModifier(type, value, source);
    }
    public void RemoveAllModifiersFromResource(ResourceDefinition definition, object source = null)
    {
        if (!TryGetResource(definition, out CharacterResource resource))
            return;

        resource.RemoveAllModifiers(source);
    }
    public void RemoveSpecificModifierFromAllResources(ResourceModifierType type, float value, object source = null) => Resources.ForEach(r => r.RemoveSpecificModifier(type, value, source));
    public void RemoveAllModifiersFromAllResources(object source = null) => Resources.ForEach(r => r.RemoveAllModifiers(source));
}