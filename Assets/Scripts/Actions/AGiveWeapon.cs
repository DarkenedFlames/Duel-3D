using UnityEngine;

[System.Serializable]
public class AGiveWeapon : IGameAction
{
    [Header("Weapon Configuration")]
    [Tooltip("Weapon to give."), SerializeField]
    GameObject weaponPrefab;

    public void Execute(ActionContext context)
    {
        if (context.Target == null)
        {
            Debug.LogError($"Action {nameof(AGiveWeapon)} was passed a null parameter: {nameof(context.Target)}");
            return;
        }
        if (weaponPrefab == null)
        {
            Debug.LogError($"Action {nameof(AGiveWeapon)} was configured with a null parameter: {nameof(weaponPrefab)}!");
            return;
        }
        if (!context.Target.TryGetComponent(out CharacterWeapons weapons))
        {
            Debug.LogError($"Action {nameof(AGiveWeapon)} was passed a paramter with a missing component: {nameof(context.Target)} missing {nameof(CharacterWeapons)}!");
            return;
        }

        weapons.EquipWeapon(weaponPrefab);
    }
}