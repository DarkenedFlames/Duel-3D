using UnityEngine;

[System.Serializable]
public class ADealsDamage : IGameAction
{
    [Header("Damage Configuration")]
    [Tooltip("Whether to reset regeneration if damage is dealt."), SerializeField]
    bool resetRegenerationIfChanged = true;

    [Tooltip("The amount of damage dealt."), SerializeField, Min(0)]
    float amount = 1f;

    [Header("Damage Number")]
    [SerializeField] GameObject damageNumberPrefab;

    public void Execute(ActionContext context)
    {
        if (context.Source == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Source), nameof(Execute), nameof(ADealsDamage), context.Source.GameObject);
            return;
        }
        if (context.Target == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Target), nameof(Execute), nameof(ADealsDamage), context.Source.GameObject);
            return;
        }

        if (Mathf.Approximately(0, amount)) return;

        CharacterResources resources = context.Target.CharacterResources;
 
        if (resources.ChangeResourceValue(
                resources.healthDefinition,
                -1f * amount * context.Magnitude,
                out float changed,
                resetRegenerationIfChanged)
        )
        {
            GameObject number = Object.Instantiate(
                damageNumberPrefab,
                context.Target.transform.position,
                context.Target.transform.rotation
            );
            number.GetComponentInChildren<DamageNumberUI>().Initialize(Mathf.Abs(changed));
        }
    }
}