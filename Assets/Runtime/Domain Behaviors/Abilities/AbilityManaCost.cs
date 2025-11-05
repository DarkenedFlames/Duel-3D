public class AbilityManaCost : AbilityBehavior<AbilityManaCostDefinition>
{
    public AbilityManaCost(AbilityManaCostDefinition def, Ability owner) : base(def, owner) { }

    public override void OnCast() =>
        Owner.Handler.gameObject.GetComponent<StatsHandler>().SpendMana(Definition.manaCost);
    
    public override void OnUpdate(float _)
    {
        var stats = Owner.Handler.gameObject.GetComponent<StatsHandler>();
        if (stats.mana < Definition.manaCost) Owner.castGate.Lock(AbilityKey.Mana);
        else Owner.castGate.Unlock(AbilityKey.Mana);
    }
}
