using UnityEngine;

[System.Serializable]
public class LDamage : EventReaction
{
    [Header("Damage Configuration")]
    [Tooltip("The amount of damage dealt."), SerializeField]
    float amount;
    public override void OnEvent(EventContext context)
    {
        if (context.defender == null) return;

        if (context.defender.TryGetComponent(out StatsHandler stats))
            stats.TakeDamage(amount);
    }
}