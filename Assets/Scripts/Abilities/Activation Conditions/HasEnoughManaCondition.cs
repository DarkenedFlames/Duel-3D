using UnityEngine;

[CreateAssetMenu(menuName = "Duel/Abilities/Conditions/HasEnoughMana")]
public class HasEnoughManaCondition : ActivationCondition
{
    public override bool IsMet(AbilityHandler handler, Ability ability)
    {
        if (!handler.StatsHandler.TryGetStat(StatType.Mana, out var val)) return false;
        else return val >= ability.Definition.manaCost; 
    }

}
