
using UnityEngine;

[CreateAssetMenu(fileName = "New Pickup Gives Weapon Behavior", menuName = "Duel/Pickups/Behaviors/GivesWeapon")]
public class PickupGivesWeaponDefinition : PickupBehaviorDefinition
{
    [Header("Weapon configs")]
    public WeaponConfig[] configs;

    public override PickupBehavior CreateRuntimeBehavior(Pickup pickup) => new PickupGivesWeapon(pickup, this);
    
}

public class PickupGivesWeapon : PickupBehavior
{
    private new PickupGivesWeaponDefinition Definition => (PickupGivesWeaponDefinition)base.Definition;
    public PickupGivesWeapon(Pickup pickup, PickupGivesWeaponDefinition definition) : base(pickup, definition) { }

    private void GiveWeapon(GameObject target, HookType type)
    {
        foreach (WeaponConfig config in Definition.configs)
            if (config.hookType.HasFlag(type))
                SpawnerController.Instance.SpawnWeapon(config.weaponPrefab, target);
    }

    public override void OnCollide(GameObject target) => GiveWeapon(target, HookType.OnCollide);
}