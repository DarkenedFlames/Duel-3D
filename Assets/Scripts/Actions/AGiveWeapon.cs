using UnityEngine;

[System.Serializable]
public class AGiveWeapon : IGameAction
{
    [Header("Target Configuration")]
    [Tooltip("Who to give weapon to: Owner (caster/summoner) or Target (hit character)."), SerializeField]
    ActionTargetMode targetMode = ActionTargetMode.Target;

    [Header("Weapon Configuration")]
    [Tooltip("Weapon to give."), SerializeField]
    GameObject weaponPrefab;

    public void Execute(ActionContext context)
    {
        if (weaponPrefab == null)
        {
            LogFormatter.LogNullField(nameof(weaponPrefab), nameof(AGiveWeapon), context.Source.GameObject);
            return;
        }

        Character target = targetMode switch
        {
            ActionTargetMode.Owner => context.Source.Owner,
            ActionTargetMode.Target => context.Target,
            _ => null,
        };

        if (target == null)
        {
            Debug.LogWarning($"{nameof(AGiveWeapon)}: {targetMode} is null. Action skipped.");
            return;
        }


        target.CharacterWeapons.EquipWeapon(weaponPrefab);
    }
}