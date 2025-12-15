using UnityEngine;
using System.Collections.Generic;

public enum ModifyResourceMode { ChangeValue, AddModifier, RemoveModifier }
public enum ModifyResourceTarget { Specific, All }
public enum ResourceModifierTarget { Specific, All, SpecificFromSource, AllFromSource }

[System.Serializable]
public class AModifyResource : IGameAction
{
    [Header("Conditions")]
    [SerializeReference]
    public List<IActionCondition> Conditions;

    [Header("Target Configuration")]
    [Tooltip("Who to modify: Owner (caster/summoner) or Target (hit character)."), SerializeField]
    ActionTargetMode targetMode = ActionTargetMode.Target;

    [Header("Resource Configuration")]
    [Tooltip("The mode of resource modification."), SerializeField]
    ModifyResourceMode mode = ModifyResourceMode.ChangeValue;

    [Tooltip("Targeting mode for which resources to modify."), SerializeField]
    ModifyResourceTarget targetResource = ModifyResourceTarget.Specific;

    [Tooltip("The resource to modify."), SerializeField]
    ResourceType resourceType;

    [Tooltip("Targeting mode for which modifiers to modify."), SerializeField]
    ResourceModifierTarget targetModifier = ResourceModifierTarget.Specific;

    [Tooltip("The type of modifier to modify."), SerializeField]
    ResourceModifierType modifierType = ResourceModifierType.Increase;

    [Tooltip("If the value changes, reset the resource's regeneration?"), SerializeField]
    bool resetRegeneration = false;

    [Tooltip("The change or modifier value."), SerializeField]
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
            case ModifyResourceMode.ChangeValue: resources.ChangeResourceValue(resourceType, amount, out float _, resetRegeneration); break;
            case ModifyResourceMode.AddModifier: resources.AddModifierToResource(resourceType, modifierType, amount, context.Source); break;
            case ModifyResourceMode.RemoveModifier:
                switch (targetResource)
                {
                    case ModifyResourceTarget.Specific:
                        switch (targetModifier)
                        {
                            case ResourceModifierTarget.Specific:           resources.RemoveSpecificModifierFromResource(resourceType, modifierType, amount, source: null); break;
                            case ResourceModifierTarget.SpecificFromSource: resources.RemoveSpecificModifierFromResource(resourceType, modifierType, amount, context.Source); break;
                            case ResourceModifierTarget.All:                resources.RemoveAllModifiersFromResource(resourceType, source: null); break;
                            case ResourceModifierTarget.AllFromSource:      resources.RemoveAllModifiersFromResource(resourceType, context.Source); break;
                        }
                        break;
                    case ModifyResourceTarget.All:
                        switch (targetModifier)
                        {
                            case ResourceModifierTarget.Specific:           resources.RemoveSpecificModifierFromAllResources(modifierType, amount, source: null); break;
                            case ResourceModifierTarget.SpecificFromSource: resources.RemoveSpecificModifierFromAllResources(modifierType, amount, context.Source); break;
                            case ResourceModifierTarget.All:                resources.RemoveAllModifiersFromAllResources(source: null); break;
                            case ResourceModifierTarget.AllFromSource:      resources.RemoveAllModifiersFromAllResources(context.Source); break;
                        }
                        break;
                }
                break;
        }
    }
}
