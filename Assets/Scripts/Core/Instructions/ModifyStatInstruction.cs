using System;
using UnityEngine;

[Serializable]
public class ModifyStatInstruction : IInstruction
{
    [Tooltip("Stat to modify.")]
    public StatType type;

    [Tooltip("Modifies max stat if true, otherwise modifies current stat.")]
    public bool modifyMax = false;

    [Tooltip("Amount by which to modify.")]
    public float amount;

    public void Execute(IInstructionContext context)
    {
        if (context.Actor.TryGetComponent(out StatsHandler stats))
            stats.TryModifyStat(type, modifyMax, amount);

    }
}