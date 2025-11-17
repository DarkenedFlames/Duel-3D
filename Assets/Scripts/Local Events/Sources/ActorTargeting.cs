using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class ActorTargeting : MonoBehaviour
{
    public event Action<GameObject> OnActorEnter;
    public event Action<GameObject> OnActorStay;
    public event Action<GameObject> OnActorExit;

    public IReadOnlyCollection<GameObject> CurrentTargets => currentTargets;

    private readonly HashSet<GameObject> currentTargets = new();

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private GameObject ColliderToActor(Collider other)
    {
        GameObject rootObj = other.transform.root.gameObject;

        if (rootObj.layer == LayerMask.NameToLayer("Actors"))
            return rootObj;

        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject actor = ColliderToActor(other);
        if (actor == null)
            return;

        if (currentTargets.Add(actor))
            OnActorEnter?.Invoke(actor);
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject actor = ColliderToActor(other);
        if (actor == null)
            return;

        if (currentTargets.Contains(actor))
            OnActorStay?.Invoke(actor);
        
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject actor = ColliderToActor(other);
        if (actor == null)
            return;

        if (currentTargets.Remove(actor))
            OnActorExit?.Invoke(actor);
        
    }
}
