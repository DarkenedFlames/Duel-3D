using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AModifyResource : IGameAction
{
    public enum Mode {
        ChangeSpecificResourceValue,
        AddModifierToSpecificResource,
        AddModifierToRandomResourceFromSet,
        AddModifierToAllResourcesFromSet,
        AddModifierToAllResources,
        RemoveSpecificModifierFromSpecificResource,
        RemoveSpecificModifierFromAllResources,
        RemoveAllModifiersFromSpecificResource,
        RemoveAllModifiersFromAllResources,
    }

    [Header("Conditions")]
    [SerializeReference]
    public List<IActionCondition> Conditions;

    [Header("Action Configuration")]
    [Tooltip("Who to modify: Owner (caster/summoner) or Target (hit character)."), SerializeField]
    ActionTargetMode targetMode = ActionTargetMode.Target;

    [Tooltip("Whether to add or remove the modifier."), SerializeField]
    Mode mode = Mode.ChangeSpecificResourceValue;

    [Tooltip("Whether to only remove modifiers that were added by this."), SerializeField]
    bool removeOnlyFromSource = true;


    [Tooltip("The resource to modify."), SerializeField]
    ResourceDefinition resourceDefinition;

    [Tooltip("The set of resource definitions to choose from."), SerializeField]
    ResourceDefinitionSet resourceDefinitions;

    [Tooltip("The type of modifier to modify."), SerializeField]
    ResourceModifierType targetModifierType = ResourceModifierType.Increase;

    [Tooltip("Whether to reset regeneration if changed."), SerializeField]
    bool resetRegeneration = false;

    [Tooltip("The value of the modifier or value delta."), SerializeField]
    float amount = 0f;


    [Tooltip("The number + icon prefab to use (NumberIconUI)"), SerializeField]
    GameObject numberIconUI;

    public void Execute(ActionContext context)
    {
        Character target = targetMode switch
        {
            ActionTargetMode.Owner => context.Source.Owner,
            ActionTargetMode.Target => context.Target,
            _ => null,
        };

        if (target == null)
        {
            Debug.LogWarning($"{nameof(AModifyResource)}: {targetMode} is null. Action skipped.");
            return;
        }

        if (Conditions != null)
        {
            foreach (IActionCondition condition in Conditions)
                if (!condition.IsSatisfied(context))
                    return;
        }
        
        Dictionary<ResourceDefinition, List<ResourceModifier>> added = new();
        Dictionary<ResourceDefinition, List<ResourceModifier>> removed = new();
        
        CharacterResources resources = target.CharacterResources;
        switch (mode)
        {
            case Mode.ChangeSpecificResourceValue:
                if (resources.ChangeResourceValue(resourceDefinition.resourceType, amount, out float delta, resetRegeneration))
                    SpawnValueChangeUI(resourceDefinition, delta, target.transform);
                break;

            case Mode.AddModifierToSpecificResource:
                added = resources.AddModifiers(targetModifierType, amount, resourceDefinition.resourceType, context.Source);
                break;

            case Mode.AddModifierToRandomResourceFromSet:
                if (resourceDefinitions.TryGetRandomResourceDefinition(out ResourceDefinition randomDefinition))
                    added = resources.AddModifiers(targetModifierType, amount, randomDefinition.resourceType, context.Source);
                break;

            case Mode.AddModifierToAllResourcesFromSet:
                foreach (ResourceDefinition definition in resourceDefinitions.Definitions)
                {
                    added[definition] = resources.AddModifiers(
                        targetModifierType, 
                        amount,
                        definition.resourceType,
                        context.Source
                    )[definition];
                }
                break;

            case Mode.AddModifierToAllResources:
                added = resources.AddModifiers(targetModifierType, amount, null, context.Source);
                break;

            case Mode.RemoveSpecificModifierFromSpecificResource:
                removed = resources.RemoveModifiers(resourceDefinition.resourceType, targetModifierType, amount, removeOnlyFromSource ? context.Source : null);
                break;

            case Mode.RemoveSpecificModifierFromAllResources:
                removed = resources.RemoveModifiers(null, targetModifierType, amount, removeOnlyFromSource ? context.Source : null);
                break;

            case Mode.RemoveAllModifiersFromSpecificResource:
                removed = resources.RemoveModifiers(resourceDefinition.resourceType, null, null, removeOnlyFromSource ? context.Source : null);
                break;

            case Mode.RemoveAllModifiersFromAllResources:
                removed = resources.RemoveModifiers(null, null, null, removeOnlyFromSource ? context.Source : null);
                break;
        }
        
        foreach (KeyValuePair<ResourceDefinition, List<ResourceModifier>> kvp in added)
            foreach (ResourceModifier modifier in kvp.Value)
                SpawnModifierUI(kvp.Key, modifier, Color.blue, target.transform);
           
        foreach (KeyValuePair<ResourceDefinition, List<ResourceModifier>> kvp in removed)
            foreach (ResourceModifier modifier in kvp.Value)
                SpawnModifierUI(kvp.Key, modifier, Color.orange, target.transform);
    }
    
    void SpawnValueChangeUI(ResourceDefinition definition, float delta, Transform targetTransform)
    {
        if (numberIconUI == null)
            return;
    
        GameObject spawnedUI = Object.Instantiate(numberIconUI, targetTransform.position, targetTransform.rotation);
        if (!spawnedUI.TryGetComponent(out NumberIconUI uiComponent))
        {
            Debug.LogError($"{spawnedUI.name} is missing {nameof(NumberIconUI)}");
            return;
        }
        
        uiComponent.Initialize(delta, delta > 0 ? Color.green : Color.red, definition.Icon, NumberIconUI.FormatMode.Flat);
    }
    
    
    void SpawnModifierUI(ResourceDefinition definition, ResourceModifier modifier, Color color, Transform targetTransform)
    {
        if (numberIconUI == null)
            return;
    
        GameObject spawnedUI = Object.Instantiate(numberIconUI, targetTransform.position, targetTransform.rotation);
        if (!spawnedUI.TryGetComponent(out NumberIconUI uiComponent))
        {
            Debug.LogError($"{spawnedUI.name} is missing {nameof(NumberIconUI)}");
            return;
        }

        Sprite icon = modifier.Type == ResourceModifierType.Increase
                ? definition.IncreaseIcon 
                : definition.DecreaseIcon;
        
        uiComponent.Initialize(modifier.Value, color, icon, NumberIconUI.FormatMode.PercentMult);
        // For stats
        // uiComponent.Initialize(modifier.Value, color, definition.Icon, (NumberIconUI.FormatMode)(int)modifier.Type);
    }
}
