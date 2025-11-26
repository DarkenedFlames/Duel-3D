using System.Linq;
using System.Net.Mime;
using UnityEngine;

[System.Serializable]
public class AModifyStatModifier : IGameAction
{
    [Header("Stat Configuration")]
    [Tooltip("Stat to modify."), SerializeField]
    StatDefinition StatDefinition;

    [Tooltip("If true, the 'maximum' stat will be modified, otherwise the 'current' stat will be modified."), SerializeField]
    bool modifyMax = false;

    [Tooltip("Whether to add or remove a modifier."), SerializeField]
    bool mode = false;

    [Tooltip("The type of modifier to modify."), SerializeField]
    StatModifierType type = StatModifierType.Flat;

    [Tooltip("The modifier value."), SerializeField]
    float amount = 0f;

    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            Debug.LogError($"Action {nameof(AModifyStatModifier)} was passed a null parameter: {nameof(context.Target)}!");
            return;
        }
        if (context.Source == null)
        {
            Debug.LogError($"Action {nameof(AModifyStatModifier)} was passed a null parameter: {nameof(context.Source)}!");
            return;
        }
        if (StatDefinition == null)
        {
            Debug.LogError($"Action {nameof(AModifyStatModifier)} was configured with a null parameter: {nameof(StatDefinition)}!");
            return;
        }
        if (!context.Target.TryGetComponent(out CharacterStats stats))
        {
            Debug.LogError($"Action {nameof(AModifyStatModifier)} was passed a parameter with a missing component: {nameof(CharacterStats)}!");
            return;
        }
        if (!stats.TryGetStat(StatDefinition.statName, out ClampedStat stat))
        {
            Debug.LogError($"Action {nameof(AModifyStat)} could not find stat: {StatDefinition.statName}!");
            return;
        }

        Stat statToChange = modifyMax ? stat.MaxStat : stat;

        if (mode)
            statToChange.AddModifier(new StatModifier(type, amount, context.Source));
        else
        {
            StatModifier toRemove = statToChange.Modifiers.ToList().Find(m => m.Value == amount && m.Type == type);
            if (toRemove == null)
                statToChange.RemoveModifier(toRemove);
        }
    }
}
