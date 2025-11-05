using UnityEngine;

[CreateAssetMenu(fileName = "New Ability Mana Cost Definition", menuName = "Abilities/Behaviors/Mana Cost")]
public class AbilityManaCostDefinition : AbilityBehaviorDefinition<AbilityManaCost, AbilityManaCostDefinition>
{
    public float manaCost = 10.0f;

    protected override AbilityManaCost CreateTypedInstance(Ability owner) => new(this, owner);
}