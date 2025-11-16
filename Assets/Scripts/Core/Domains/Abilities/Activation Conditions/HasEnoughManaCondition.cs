using UnityEngine;

[CreateAssetMenu(menuName = "Duel/Abilities/Conditions/HasEnoughMana")]
public class HasEnoughManaCondition : ActivationCondition
{
    public override bool IsMet(Ability ability)
    {
        if (!ability.Handler.TryGetComponent(out StatsHandler stats))
            return false;

        float currentMana = stats.GetStat(StatType.Mana, getMax: false);
        return currentMana >= ability.Definition.manaCost; 
    }

}
