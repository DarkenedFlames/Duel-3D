using System.Collections.Generic;

[System.Serializable]
public abstract class EventReaction
{
    public List<Event> Events;
    public abstract void OnEvent(EventContext ctx);
}
