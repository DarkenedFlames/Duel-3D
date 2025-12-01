using UnityEngine;

[System.Serializable]
public class ADealsDamage : IGameAction
{
    [Header("Damage Configuration")]
    [Tooltip("The amount of damage dealt."), SerializeField, Min(0)]
    int amount = 1;

    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Target), nameof(Execute), nameof(ADealsDamage), context.Source.GameObject);
            return;
        }

        context.Target.CharacterResources.TakeDamage(amount * context.Magnitude);
    }
}