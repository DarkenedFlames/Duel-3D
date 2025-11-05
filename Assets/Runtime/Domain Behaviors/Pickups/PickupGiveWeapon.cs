using UnityEngine;

public class PickupGiveWeapon : PickupBehavior<PickupGiveWeaponDefinition>
{
    public PickupGiveWeapon(PickupGiveWeaponDefinition def, Pickup owner) : base(def, owner) { }
    public override void OnTrigger(Collider other)
    {
        SpawnerController.Instance.SpawnWeapon(Definition.weaponPrefab, other.gameObject);
        Owner.Handler.Expire();
    }
}
