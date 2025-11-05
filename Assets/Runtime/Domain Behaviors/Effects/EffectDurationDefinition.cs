using UnityEngine;

[CreateAssetMenu(fileName = "New Effect Duration Behavior", menuName = "Effects/Behaviors/Duration")]
public class EffectDurationDefinition : EffectBehaviorDefinition<EffectDuration, EffectDurationDefinition>
{
    [Header("Duration Settings")]
    public float duration = 5f;
    protected override EffectDuration CreateTypedInstance(Effect owner) => new(this, owner);
}