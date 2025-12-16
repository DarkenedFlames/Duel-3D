using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterResources : MonoBehaviour
{
    [SerializeField] ResourceDefinitionSet initialResources;
    readonly List<CharacterResource> resources = new();

    bool initialized = false;

    public event Action<CharacterResource> OnResourceValueChanged;
    public event Action<GameObject> OnDeath;

    Character Owner;

    void Awake() => Owner = GetComponent<Character>();
    void Start() => EnsureInitialized();
    public void EnsureInitialized()
    {
        if (initialized) return;

        foreach (ResourceDefinition definition in initialResources.Definitions)
            resources.Add(new(definition, Owner.CharacterStats.GetStat(definition.MaxStat.statType)));

        initialized = true;
    }

    public CharacterResource GetResource(ResourceType type) => resources.Find(r => r.Definition.resourceType == type);
    
    void Update()
    {
        foreach (CharacterResource resource in resources)
            if (resource.TickRegeneration(out _))
                OnResourceValueChanged?.Invoke(resource);
        TryDie();
    }

    void TryDie()
    {
        if (GetResource(ResourceType.Health).Value > 0) return;
        
        Debug.Log($"{gameObject.name} died!");
        OnDeath?.Invoke(gameObject);
        Destroy(gameObject);
    }

    public bool ChangeResourceValue(ResourceType type, float delta, out float changed, bool resetRegenerationIfChanged = false)
    {
        CharacterResource resource = GetResource(type);

        bool didChange = resource.ChangeValue(delta, out changed);
        if (didChange)
        {
            OnResourceValueChanged?.Invoke(resource);
            if (resetRegenerationIfChanged)
                resource.RegenerationCounter.Reset();
        }
        return didChange;
    }

    public void AddModifiers(ResourceModifierType modifierType, float modifierValue, ResourceType? resourceType = null,  object source = null)
    {
        foreach (CharacterResource resource in resources)
            if (resourceType == null || GetResource(resourceType.Value) == resource)
                resource.AddModifier(new ResourceModifier(modifierType, modifierValue, source));
    }

    public void RemoveModifiers(ResourceType? resourceType = null, ResourceModifierType? modifierType = null, float? modifierValue = null, object source = null)
    {
        foreach (CharacterResource resource in resources)
            if (resourceType == null || GetResource(resourceType.Value) == resource)
                resource.RemoveModifiers(modifierType, modifierValue, source);
    }
}