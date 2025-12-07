using UnityEngine;

public enum ModifyResourceMode { ChangeValue, AddModifier, RemoveModifier }
public enum ModifyResourceTarget { Specific, All }
public enum ResourceModifierTarget { Specific, All, SpecificFromSource, AllFromSource }

[System.Serializable]
public class AModifyResource : IGameAction
{
    [Header("Resource Configuration")]
    [Tooltip("The mode of resource modification."), SerializeField]
    ModifyResourceMode mode = ModifyResourceMode.ChangeValue;

    [Tooltip("Targeting mode for which resources to modify."), SerializeField]
    ModifyResourceTarget target = ModifyResourceTarget.Specific;

    [Tooltip("The resource to modify."), SerializeField]
    ResourceDefinition resourceDefinition;

    [Tooltip("Targeting mode for which modifiers to modify."), SerializeField]
    ResourceModifierTarget modifierTarget = ResourceModifierTarget.Specific;

    [Tooltip("The type of modifier to modify."), SerializeField]
    ResourceModifierType type = ResourceModifierType.Increase;

    [Tooltip("If the value changes, reset the resource's regeneration?"), SerializeField]
    bool resetRegeneration = false;

    [Tooltip("The change or modifier value."), SerializeField]
    float amount = 0f;

    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Target), nameof(Execute), nameof(AModifyResource), context.Source.GameObject);
            return;
        }
        if (context.Source == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Source), nameof(Execute), nameof(AModifyResource), context.Source.GameObject);
            return;
        }
        
        CharacterResources resources = context.Target.CharacterResources;
        switch (mode)
        {
            case ModifyResourceMode.ChangeValue: resources.ChangeResourceValue(resourceDefinition, amount, out float _, resetRegeneration); break;
            case ModifyResourceMode.AddModifier: resources.AddModifierToResource(resourceDefinition, type, amount, context.Source); break;
            case ModifyResourceMode.RemoveModifier:
                switch (target)
                {
                    case ModifyResourceTarget.Specific:
                        switch (modifierTarget)
                        {
                            case ResourceModifierTarget.Specific:           resources.RemoveSpecificModifierFromResource(resourceDefinition, type, amount, source: null); break;
                            case ResourceModifierTarget.SpecificFromSource: resources.RemoveSpecificModifierFromResource(resourceDefinition, type, amount, context.Source); break;
                            case ResourceModifierTarget.All:                resources.RemoveAllModifiersFromResource(resourceDefinition, source: null); break;
                            case ResourceModifierTarget.AllFromSource:      resources.RemoveAllModifiersFromResource(resourceDefinition, context.Source); break;
                        }
                        break;
                    case ModifyResourceTarget.All:
                        switch (modifierTarget)
                        {
                            case ResourceModifierTarget.Specific:           resources.RemoveSpecificModifierFromAllResources(type, amount, source: null); break;
                            case ResourceModifierTarget.SpecificFromSource: resources.RemoveSpecificModifierFromAllResources(type, amount, context.Source); break;
                            case ResourceModifierTarget.All:                resources.RemoveAllModifiersFromAllResources(source: null); break;
                            case ResourceModifierTarget.AllFromSource:      resources.RemoveAllModifiersFromAllResources(context.Source); break;
                        }
                        break;
                }
                break;
        }
    }
}
