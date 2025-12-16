using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AGiveWeapon : IGameAction
{
    [Header("Conditions")]
    [SerializeReference]
    public List<IActionCondition> Conditions;

    [Header("Action Configuration")]
    [Tooltip("Who to give weapon to: Owner (caster/summoner) or Target (hit character)."), SerializeField]
    ActionTargetMode targetMode = ActionTargetMode.Target;

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

        if (Conditions != null)
        {
            foreach (IActionCondition condition in Conditions)
                if (!condition.IsSatisfied(context))
                    return;
        }

        target.CharacterWeapons.EquipWeapon(weaponPrefab);
    }
}