using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Conditions/HasEnoughMana")]
public class HasEnoughManaCondition : ActivationCondition
{
    public override bool IsMet(Ability ability)
    {
        if (!ability.GameObject.TryGetComponent(out CharacterStats stats)) return false;
        if (!stats.TryGetStat("Mana", out ClampedStat mana)) return false;

        return mana.Value >= ability.Definition.manaCost; 
    }
}
