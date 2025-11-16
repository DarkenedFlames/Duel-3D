
using UnityEngine;

public class Ability
{
    public AbilityDefinition Definition { get; private set; }
    public AbilityHandler Handler { get; private set; }
    public float cooldownRemaining;

    public Ability(AbilityDefinition definition, AbilityHandler handler)
    {
        Definition = definition;
        Handler = handler;
        cooldownRemaining = 0f;
    }

    public bool ResetCooldown()
    {
        bool isReady = cooldownRemaining <= 0f;
        if (isReady)
            cooldownRemaining = Definition.cooldown;
        return isReady;
    }

    public void TickCooldown(float dt)
    {
        if (cooldownRemaining > 0f) cooldownRemaining = Mathf.Max(0f, cooldownRemaining - dt);
    }
}
