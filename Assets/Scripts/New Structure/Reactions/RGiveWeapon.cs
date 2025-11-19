using UnityEngine;

public class RGiveWeapon : Reaction
{
    [Header("GiveWeapon Configuration")]
    [Tooltip("Weapon to give."), SerializeField]
    GameObject weaponPrefab;

    public void GiveWeapon(GameObject target)
    {
        if (target == null) return;

        if (target.TryGetComponent(out ActorWeaponHandler weaponHandler))
            weaponHandler.EquipWeapon(weaponPrefab);
    }
}