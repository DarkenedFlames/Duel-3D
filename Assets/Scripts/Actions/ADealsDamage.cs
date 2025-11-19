using UnityEngine;

[System.Serializable]
public class ADealsDamage : IGameAction
{
    [Header("Damage Configuration")]
    [Tooltip("The amount of damage dealt."), SerializeField]
    int amount;

    public void Execute(GameObject target)
    {
        if (target == null) return;

        if (target.TryGetComponent(out StatsHandler stats))
            stats.TakeDamage(amount);
    }
}