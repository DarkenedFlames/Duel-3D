using UnityEngine;

[System.Serializable]
public class AApplyResourceModifier : IGameAction
{
    [Header("Resource Configuration")]
    [Tooltip("Resource to modify."), SerializeField]
    ResourceDefinition ResourceDefinition;

    [Tooltip("The type of modifier to apply."), SerializeField]
    ResourceModifierType type = ResourceModifierType.Increase;

    [Tooltip("The modifier value."), SerializeField]
    float amount = 0f;

    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            LogFormatter.LogNullField(nameof(context.Target), nameof(AApplyResourceModifier), context.Source.GameObject);
            return;
        }
        if (context.Source == null)
        {
            LogFormatter.LogNullField(nameof(context.Source), nameof(AApplyResourceModifier), context.Source.GameObject);
            return;
        }
        if (ResourceDefinition == null)
        {
            LogFormatter.LogNullField(nameof(AApplyResourceModifier), nameof(ResourceDefinition), context.Source.GameObject);
            return;
        }
        if (!context.Target.CharacterResources.TryGetResource(ResourceDefinition, out CharacterResource resource))
        {
            Debug.LogWarning($"Action {nameof(AApplyResourceModifier)} could not find resource from definition {ResourceDefinition.name}!", context.Source.GameObject);
            return;
        }

        // Want to add magnitude, but not sure if it works properly with effects and gaining new stacks.
        resource.AddModifier(new ResourceModifier(type, amount, context.Source));
    }
}
