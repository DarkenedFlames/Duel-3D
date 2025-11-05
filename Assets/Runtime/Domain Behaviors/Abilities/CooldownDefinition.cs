using UnityEngine;

[CreateAssetMenu(fileName = "New Cooldown", menuName = "Abilities/Behaviors/Cooldown")]
public class CooldownDefinition : AbilityBehaviorDefinition<CooldownBehavior, CooldownDefinition>
{
    [Header("Cooldown Settings")]
    public float cooldown = 1f;

    protected override CooldownBehavior CreateTypedInstance(Ability owner) => new(this, owner);
}
