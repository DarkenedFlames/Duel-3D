using UnityEngine;

[System.Serializable]
public class LModifyStat : EventReaction // MonoBehaviour
{
    [Header("ModifyStat Configuration")]
    [Tooltip("Stat to modify."), SerializeField]
    StatType type;

    [Tooltip("If true, the 'maximum' stat will be modified, otherwise the 'current' stat will be modified."), SerializeField]
    bool modifyMax = false;

    [Tooltip("Amount by which to modify."), SerializeField]
    float amount;

    public override void OnEvent(EventContext context)
    {
        if (context is PositionContext cxt)
            if (cxt.target.TryGetComponent(out StatsHandler stats))
                stats.TryModifyStat(type, modifyMax: modifyMax, amount);
    }
}
