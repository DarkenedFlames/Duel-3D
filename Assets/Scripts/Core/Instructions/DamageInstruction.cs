using System;
using UnityEngine;

[Serializable]
public class DamageInstruction : IInstruction
{
    public float amount;
    public void Execute(IInstructionContext context)
    {
        if (context.Actor.TryGetComponent(out StatsHandler stats))
            stats.TakeDamage(amount);
    }
}