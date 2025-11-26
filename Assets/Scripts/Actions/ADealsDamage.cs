using UnityEngine;

[System.Serializable]
public class ADealsDamage : IGameAction
{
    [Header("Damage Configuration")]
    [Tooltip("The amount of damage dealt."), SerializeField]
    int amount;

    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            Debug.LogError($"Action {nameof(ADealsDamage)} was passed a null parameter: {nameof(context.Target)}!");
            return;
        }
        if (amount <= 0)
        {
            Debug.LogError($"Action {nameof(ADealsDamage)} was configured with an invalid parameter: {nameof(amount)} must be positive!");
            return;
        }
        if (!context.Target.TryGetComponent(out CharacterStats stats))
        {
            Debug.LogError($"Action {nameof(ADealsDamage)} was passed a parameter with a missing component: {nameof(context.Target)} missing {nameof(CharacterStats)}!");
            return;
        }

        stats.TakeDamage(amount);
    }
}