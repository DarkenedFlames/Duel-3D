using UnityEngine;

public class RModifyStat : Reaction
{
    [Header("ModifyStat Configuration")]
    [Tooltip("Stat to modify."), SerializeField]
    StatType type;

    [Tooltip("If true, the 'maximum' stat will be modified, otherwise the 'current' stat will be modified."), SerializeField]
    bool modifyMax = false;

    [Tooltip("Amount by which to modify."), SerializeField]
    float amount;

    public void ModifyStat(GameObject target)
    {
        if (target == null) return;

        if (target.TryGetComponent(out StatsHandler stats))
            stats.TryModifyStat(type, modifyMax: modifyMax, amount);
    }
}
