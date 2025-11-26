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
        foreach (ActivationCondition condition in Definition.activationConditions)
            if (!condition.IsMet(this)) return false;

        if (!seconds.Expired) return false;

        if(GameObject.TryGetComponent(out CharacterStats stats) && stats.TryGetStat("Mana", out ClampedStat stat))
            stat.BaseValue -= Definition.manaCost;

        seconds.Reset();
        ActionContext context = new(){ Source = this, Target = GameObject.GetComponent<Character>() };
        Definition.actions.ForEach(a => a.Execute(context));
        return true;
    }
}
