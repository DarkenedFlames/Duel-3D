using System;
using UnityEngine;

public class Ability
{
    public AbilityDefinition Definition { get; private set; }
    public CharacterAbilities Handler { get; private set; }
    readonly Counter seconds;
    public event Action<Ability> OnAbilityActivated;

    public Ability(AbilityDefinition definition, CharacterAbilities handler)
    {
        Definition = definition;
        Handler = handler;
        seconds = new(Definition.cooldown);
    }

    public void TickCooldown(float dt) => seconds.Decrease(dt);

    public bool TryActivate()
    {
        Debug.Log("Trying To activate!");
        foreach (ActivationCondition condition in Definition.activationConditions)
            if (!condition.IsMet(this)) return false;

        if (!seconds.Expired) return false;

        seconds.Reset();
        Definition.actions.ForEach(a => a.Execute(Handler.gameObject, Handler.gameObject));
        OnAbilityActivated?.Invoke(this);
        Debug.Log("Ability Activated!");
        return true;
    }
}
