using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class PhysicsTrigger : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Whether to execute actions on the objects that enter this object."), SerializeField]
    bool triggerOnEnter = false;

    [Tooltip("Whether to execute actions on the objects that exit this object."), SerializeField]
    bool triggerOnExit = false;

    [Header("Actions")]
    [Tooltip("The actions to execute upon entry/exit."), SerializeReference]
    List<IGameAction> actions = new();

    void OnTriggerEnter(Collider other)
    {
        if (triggerOnEnter && actions.Count != 0)
            foreach (IGameAction action in actions)
                action.Execute(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (triggerOnExit && actions.Count != 0)
            foreach (IGameAction action in actions)
                action.Execute(other.gameObject);
    }

}