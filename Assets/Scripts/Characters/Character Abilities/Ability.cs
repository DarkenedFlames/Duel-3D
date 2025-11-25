using UnityEngine;

public class Ability
{
    public GameObject GameObject { get; private set; }
    public AbilityDefinition Definition { get; private set; }
    readonly Counter seconds;

    public Ability(GameObject gameObject, AbilityDefinition definition)
    {
        GameObject = gameObject;
        Definition = definition;
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
        Definition.actions.ForEach(a => a.Execute(GameObject, GameObject));
        Debug.Log("Ability Activated!");
        return true;
    }
}
