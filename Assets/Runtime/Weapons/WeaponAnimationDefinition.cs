using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Animation Definition", menuName = "Weapons/Behaviors/Animation")]
public class WeaponAnimationDefinition : WeaponBehaviorDefinition<WeaponAnimation, WeaponAnimationDefinition>
{
    public string attackTrigger = "AttackTrigger";
    protected override WeaponAnimation CreateTypedInstance(Weapon owner) => new(this, owner);
}