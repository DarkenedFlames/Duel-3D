using System.Linq;
using System.Net.Mime;
using UnityEngine;

[System.Serializable]
public class AModifyStatModifier : IGameAction
{
    [Header("Stat Configuration")]
    [Tooltip("Stat to modify."), SerializeField]
    StatDefinition StatDefinition;

    [Tooltip("The type of modifier to apply."), SerializeField]
    StatModifierType type = StatModifierType.Flat;

    [Tooltip("The modifier value."), SerializeField]
    float amount = 0f;

    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            LogFormatter.LogNullField(nameof(context.Target), nameof(AModifyStatModifier), context.Source.GameObject);
            return;
        }
        if (context.Source == null)
        {
            LogFormatter.LogNullField(nameof(context.Source), nameof(AModifyStatModifier), context.Source.GameObject);
            return;
        }
        if (StatDefinition == null)
        {
            LogFormatter.LogNullField(nameof(AModifyStatModifier), nameof(StatDefinition), context.Source.GameObject);
            return;
        }
        if (!context.Target.CharacterStats.TryGetStat(StatDefinition, out Stat stat))
        {
            Debug.LogWarning($"Action {nameof(AModifyStatModifier)} could not find stat from definition {StatDefinition.name}!", context.Source.GameObject);
            return;
        }

        // Want to add magnitude, but not sure if it works properly with effects and gaining new stacks.
        stat.AddModifier(new StatModifier(type, amount, context.Source));
    }
}
