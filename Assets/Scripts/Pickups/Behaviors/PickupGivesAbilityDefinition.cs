
using UnityEngine;

[CreateAssetMenu(fileName = "New Pickup Gives Ability Behavior", menuName = "Duel/Pickups/Behaviors/GivesAbility")]
public class PickupGivesAbilityDefinition : PickupBehaviorDefinition
{
    [Header("Ability configs")]
    public AbilityConfig[] configs;

    public override PickupBehavior CreateRuntimeBehavior(Pickup pickup) => new PickupGivesAbility(pickup, this);
    
}

public class PickupGivesAbility : PickupBehavior
{
    private new PickupGivesAbilityDefinition Definition => (PickupGivesAbilityDefinition)base.Definition;
    public PickupGivesAbility(Pickup pickup, PickupGivesAbilityDefinition definition) : base(pickup, definition) { }

    private void GiveAbility(GameObject target, HookType type)
    {
        foreach (AbilityConfig config in Definition.configs)
            if (config.hookType.HasFlag(type) && target.TryGetComponent(out AbilityHandler abilityHandler))
                abilityHandler.LearnAbility(config.abilityDefinition);
    }

    public override void OnCollide(GameObject target) => GiveAbility(target, HookType.OnCollide);
}