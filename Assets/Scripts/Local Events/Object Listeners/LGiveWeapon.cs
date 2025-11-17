using UnityEngine;

[System.Serializable]
public class LGiveWeapon : EventReaction
{
    [Header("GiveWeapon Configuration")]
    [Tooltip("Weapon to give."), SerializeField]
    GameObject weaponPrefab;

    public override void OnEvent(EventContext context)
    {
        if (context is TargetContext cxt)
            if (cxt.target.TryGetComponent(out ActorWeaponHandler weaponHandler))
                weaponHandler.EquipWeapon(weaponPrefab);
    }
}