using UnityEngine;

[CreateAssetMenu(fileName = "New Effect Damage Behavior", menuName = "Effects/Behaviors/Damage")]
public class EffectDamageDefinition : EffectBehaviorDefinition<EffectDamage, EffectDamageDefinition>
{
    [Header("Damage Settings")]
    public float initialDamage = 0f;
    public float damagePerSecond = 5f;
    public float expireDamage = 0f;
    protected override EffectDamage CreateTypedInstance(Effect owner) => new(this, owner);
}