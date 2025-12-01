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
            LogFormatter.LogNullArgument(nameof(context.Target), nameof(Execute), nameof(AGiveWeapon), context.Source.GameObject);
            return;
        }
        if (weaponPrefab == null)
        {
            LogFormatter.LogNullField(nameof(weaponPrefab), nameof(AGiveWeapon), context.Source.GameObject);
            return;
        }

        context.Target.CharacterWeapons.EquipWeapon(weaponPrefab);
    }
}