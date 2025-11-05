using UnityEngine;

[CreateAssetMenu(fileName = "New Give Weapon Behavior", menuName = "Pickups/Behaviors/Give Weapon")]
public class PickupGiveWeaponDefinition : PickupBehaviorDefinition<PickupGiveWeapon, PickupGiveWeaponDefinition>
{
    [Header("Weapon to Give")]
    public GameObject weaponPrefab;
    protected override PickupGiveWeapon CreateTypedInstance(Pickup owner) => new(this, owner);
}
