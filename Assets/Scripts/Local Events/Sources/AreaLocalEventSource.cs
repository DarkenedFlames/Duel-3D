using UnityEngine;

public class AreaLocalEventSource : LocalEventSource
{
    
    void OnDestroy()
    {
        if (TryGetComponent(out CollisionCollector collector))
            foreach (Collider col in collector.currentTargets)
                Fire(Event.OnDestroy, new EventContext{ source = gameObject, defender = col.gameObject });
    }
}
