
using UnityEngine;

public class Ability
{
    public AbilityDefinition Definition { get; private set; }
    public AbilityHandler Handler { get; private set; }
    readonly Counter seconds;

    public Ability(AbilityDefinition definition, AbilityHandler handler)
    {
        Definition = definition;
        Handler = handler;
        seconds = new(Definition.cooldown);
    }

    public bool ResetCooldown()
    {
        if (seconds.Expired) seconds.Reset();
        return seconds.Expired;
    }

    public void TickCooldown(float dt) => seconds.Decrease(dt);
    
}
