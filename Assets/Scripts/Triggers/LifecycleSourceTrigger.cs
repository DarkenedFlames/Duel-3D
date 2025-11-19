using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(RequiresSource))]
public class LifecycleSourceTrigger : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Whether or not to execute actions on the object's source upon instantiation."), SerializeField]
    bool triggerOnAwake = false;

    [Tooltip("Whether or not to execute actions on the object's source upon destruction."), SerializeField]
    bool triggerOnDestroy = false;

    [Header("Actions")]
    [Tooltip("Actions to execute on this object's source."), SerializeReference]
    List<IGameAction> actions = new();

    GameObject source;

    void Awake()
    {
        source = GetComponent<RequiresSource>().Source;

        if (source != null)
            foreach (IGameAction action in actions)
                action.Execute(source);
    }

    void OnDestroy()
    {
        if (source != null)
            foreach (IGameAction action in actions)
                action.Execute(source);
    }
}