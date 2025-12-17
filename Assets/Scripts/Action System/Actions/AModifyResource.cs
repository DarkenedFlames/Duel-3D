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
        
        CharacterResources resources = target.CharacterResources;
        switch (mode)
        {
            case Mode.ChangeSpecificResourceValue:
                resources.ChangeResourceValue(resourceDefinition.resourceType, amount, out _, resetRegeneration);
                break;

            case Mode.AddModifierToSpecificResource:
                resources.AddModifiers(targetModifierType, amount, resourceDefinition.resourceType, context.Source);
                break;

            case Mode.AddModifierToRandomResourceFromSet:
                if (resourceDefinitions.TryGetRandomResourceDefinition(out ResourceDefinition randomDefinition))
                    resources.AddModifiers(targetModifierType, amount, randomDefinition.resourceType, context.Source);
                break;

            case Mode.AddModifierToAllResourcesFromSet:
                foreach (ResourceDefinition definition in resourceDefinitions.Definitions)
                    resources.AddModifiers(targetModifierType, amount, definition.resourceType, context.Source);
                break;

            case Mode.AddModifierToAllResources:
                resources.AddModifiers(targetModifierType, amount, null, context.Source);
                break;

            case Mode.RemoveSpecificModifierFromSpecificResource:
                resources.RemoveModifiers(resourceDefinition.resourceType, targetModifierType, amount, removeOnlyFromSource ? context.Source : null);
                break;

            case Mode.RemoveSpecificModifierFromAllResources:
                resources.RemoveModifiers(null, targetModifierType, amount, removeOnlyFromSource ? context.Source : null);
                break;

            case Mode.RemoveAllModifiersFromSpecificResource:
                resources.RemoveModifiers(resourceDefinition.resourceType, null, null, removeOnlyFromSource ? context.Source : null);
                break;

            case Mode.RemoveAllModifiersFromAllResources:
                resources.RemoveModifiers(null, null, null, removeOnlyFromSource ? context.Source : null);
                break;
        }
    }
}
