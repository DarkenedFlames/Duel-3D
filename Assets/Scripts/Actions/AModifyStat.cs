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

    public void Execute(GameObject source, GameObject target)
    {
        if (target == null) return;

        if (target.TryGetComponent(out CharacterStats stats) && stats.TryGetStat(StatDefinition.statName, out ClampedStat stat))
        {
            if (modifyMax) stat.MaxStat.BaseValue -= amount;
            else stat.BaseValue += amount;
        }
    }
}
