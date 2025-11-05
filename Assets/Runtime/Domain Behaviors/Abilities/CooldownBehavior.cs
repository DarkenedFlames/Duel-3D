public class CooldownBehavior : AbilityBehavior<CooldownDefinition>
{
    public CooldownBehavior(CooldownDefinition def, Ability owner) : base(def, owner) { }
    public override void OnUpdate(float _) {}
    public override void OnCast() => Owner.castGate.Lock(AbilityKey.Cooldown, Definition.cooldown);
}
