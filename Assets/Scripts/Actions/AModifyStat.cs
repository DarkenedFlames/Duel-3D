using UnityEngine;

[System.Serializable]
public class AModifyStat : IGameAction
{
    [Header("Stat Configuration")]
    [Tooltip("Stat to modify."), SerializeField]
    StatDefinition StatDefinition;

    [Tooltip("If true, the 'maximum' stat will be modified, otherwise the 'current' stat will be modified."), SerializeField]
    bool modifyMax = false;

    [Tooltip("Amount by which to modify."), SerializeField]
    float amount;

    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            Debug.LogError($"Action {nameof(AModifyStat)} was passed a null parameter: {nameof(context.Target)}!");
            return;
        }
        if (StatDefinition == null)
        {
            Debug.LogError($"Action {nameof(AModifyStat)} was configured with a null parameter: {nameof(StatDefinition)}!");
            return;
        }
        if (!context.Target.TryGetComponent(out CharacterStats stats))
        {
            Debug.LogError($"Action {nameof(AModifyStat)} was passed a parameter with a missing component: {nameof(CharacterStats)}!");
            return;
        }
        if (!stats.TryGetStat(StatDefinition.statName, out ClampedStat stat))
        {
            Debug.LogError($"Action {nameof(AModifyStat)} could not find stat: {StatDefinition.statName}!");
            return;
        }
        
        if (modifyMax)
            stat.MaxStat.BaseValue += amount;
        else
            stat.BaseValue += amount;
    }
}
