using UnityEngine;

public class EventContext {}

public class NullContext : EventContext {}

public class TargetContext : EventContext
{
    public GameObject target;
}

public class PositionContext : EventContext
{
    public GameObject target;
    public Transform localTransform;
}