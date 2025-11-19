using UnityEngine;

[System.Serializable]
public class AGiveWeapon : IGameAction
{
    [Header("Weapon Configuration")]
    [Tooltip("Weapon to give."), SerializeField]
    GameObject weaponPrefab;

    public void Execute(GameObject target)
    {
        if (target == null) return;

        if (target.TryGetComponent(out ActorWeaponHandler weaponHandler))
            weaponHandler.EquipWeapon(weaponPrefab);
    }
}