using UnityEngine;

[System.Serializable]
public class ADealsDamage : IGameAction
{
    [Header("Damage Configuration")]
    [Tooltip("The amount of damage dealt."), SerializeField]
    int amount;

    public void Execute(GameObject source, GameObject target)
    {
        if (target == null) return;

        if (target.TryGetComponent(out CharacterStats stats))
        {
            stats.TakeDamage(amount);
        }
    }
}