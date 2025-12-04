using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterResources : MonoBehaviour
{
    public List<ResourceDefinition> InitialResources;
    public ResourceDefinition healthDefinition; // Must be present in InitialResources

    public List<CharacterResource> Resources { get; private set; } = new();

    public event Action<CharacterResource> OnResourceLearned;
    public event Action<GameObject> OnDeath;
    public event Action<GameObject> OnTakeDamage;


    void Awake()
    {
        if (!InitialResources.Contains(healthDefinition) || healthDefinition == null)
            Debug.LogError($"{gameObject.name}'s {nameof(CharacterResources)} was configured with an invalid parameter: {nameof(InitialResources)} must reference {nameof(healthDefinition)} but it was missing or null!");
    }

    // Must run in start because it depends on CharacterStats.Awake()
    // UI.Start() is set to +1 execution order so that this will run first.
    void Start()
    {
        foreach (ResourceDefinition definition in InitialResources)
        {
            // This accesses CharacterStats.Stats before CharacterStats.Awake populates that list.
            if (!TryLearnResource(definition, out CharacterResource newResource))
                continue;
        }
    }

    public bool TryGetResource(ResourceDefinition definition, out CharacterResource resource)
    {
        resource = Resources.Find(r => r.Definition == definition);
        return resource != null;
    }

    bool TryLearnResource(ResourceDefinition definition, out CharacterResource newResource)
    {
        newResource = null;
        if (TryGetResource(definition, out CharacterResource _))
            Debug.LogWarning($"{gameObject.name}'s {nameof(CharacterResources)} tried to learn a duplicate resource from definition {definition.name}!");
        else
        {
            CharacterStats stats = GetComponent<Character>().CharacterStats;
            if (!stats.TryGetStat(definition.MaxStat, out Stat maxStat))
            {
                Debug.LogError($"{gameObject.name}'s {nameof(CharacterResources)} could not find MaxStat for resource definition {definition.name}!");
                return false;
            }

            newResource = new(definition, maxStat);
            Resources.Add(newResource);
            OnResourceLearned?.Invoke(newResource);
        }

        return newResource != null;
    }
    
    void Update()
    {
        Resources.ForEach(r => r.TickRegeneration());
        TryDie();
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0) return;
        if (!TryGetResource(healthDefinition, out CharacterResource health))
        {
            Debug.LogWarning($"{gameObject.name} could not find stat from Definition {healthDefinition.name} for damage!");
            return;
        }

        if (health.ChangeValue(-1f * amount, out float changed))
        {
            health.RegenerationCounter.Reset();
            Debug.Log($"{gameObject.name} took {amount} damage!");
            OnTakeDamage?.Invoke(gameObject);
        }
    }

    void TryDie()
    {
        if (!TryGetResource(healthDefinition, out CharacterResource health) || health.Value > 0)
            return;
        
        Debug.Log($"{gameObject.name} died!");
        OnDeath?.Invoke(gameObject);
        Destroy(gameObject);
    }

    public void ChangeResourceValue(ResourceDefinition definition, float delta, out float changed)
    {
        changed = 0;
        if (!TryGetResource(definition, out CharacterResource resource))
            return;

        resource.ChangeValue(delta, out changed);
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