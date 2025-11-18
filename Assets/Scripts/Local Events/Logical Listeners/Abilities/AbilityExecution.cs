using UnityEngine;

public class AbilityExecution
{
    public Ability Ability { get; private set; }
    public AbilityExecution(Ability ability) => Ability = ability;

    public void Activate()
    {        
        GameObject target = Ability.Handler.gameObject;
        Invoke(Event.OnActivate, new EventContext{ source = target, attacker = target, defender = target });
    }

    public void Invoke(Event evt, EventContext context)
    {
        foreach (EventReaction reaction in Ability.Definition.reactions)
            if (reaction.Events.Contains(evt))
                reaction.OnEvent(context);
    }
}
