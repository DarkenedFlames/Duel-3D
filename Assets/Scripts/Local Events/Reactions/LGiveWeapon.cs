using UnityEngine;

[System.Serializable]
public class LGiveWeapon : EventReaction
{
    [Header("GiveWeapon Configuration")]
    [Tooltip("Weapon to give."), SerializeField]
    GameObject weaponPrefab;

    public override void OnEvent(EventContext context)
    {
        if (context.defender == null) return;

        if (context.defender.TryGetComponent(out ActorWeaponHandler weaponHandler))
            weaponHandler.EquipWeapon(weaponPrefab);
    }
}