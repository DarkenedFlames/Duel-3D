using UnityEngine;

[CreateAssetMenu(menuName = "Duel/Abilities/Conditions/HasEnoughMana")]
public class HasEnoughManaCondition : ActivationCondition
{
    public override bool IsMet(Ability ability)
    {
        if (!ability.Handler.TryGetComponent(out CharacterStats stats)) return false;
        if (!stats.TryGetStat("Mana", out ClampedStat mana)) return false;

        return mana.Value >= ability.Definition.manaCost; 
    }
}
