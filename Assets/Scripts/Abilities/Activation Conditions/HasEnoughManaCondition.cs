using UnityEngine;

[CreateAssetMenu(menuName = "Duel/Abilities/Conditions/HasEnoughMana")]
public class HasEnoughManaCondition : ActivationCondition
{
    public override bool IsMet(AbilityHandler handler, Ability ability)
    {
        float currentMana = handler.StatsHandler.GetStat(StatType.Mana, getMax: false);
        return currentMana >= ability.Definition.manaCost; 
    }

}
