using System;
using UnityEngine.UIElements;

[Serializable]
public class AbilityExecution
{
    public Ability Ability { get; private set; }
    public bool IsActive { get; private set; } = false;

    public AbilityExecution(Ability ability) => Ability = ability;

    public void Activate()
    {
        if (IsActive) return;
        IsActive = true;

        PositionContext cxt = new()
        {
            target = Ability.Handler.gameObject, 
            localTransform = Ability.Handler.transform
        };

        Invoke(Event.OnActivate, cxt);
    }

    public void Invoke(Event evt, EventContext context)
    {
        foreach (EventReaction reaction in Ability.Definition.reactions)
            if (reaction.Events.Contains(evt))
                reaction.OnEvent(context);
    }

    /*
    public void Pulse()
    {
        PositionContext cxt = new()
        {
            target = Ability.Handler.gameObject, 
            localTransform = Ability.Handler.transform
        };

        Fire(Event.OnPulse, cxt);
    }
    
    public void End()
    {
        PositionContext cxt = new()
        {
            target = Ability.Handler.gameObject, 
            localTransform = Ability.Handler.transform
        };

        Fire(Event.OnEnd, cxt);
        IsActive = false;
    }
    */
}
