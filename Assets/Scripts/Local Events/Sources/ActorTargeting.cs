using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class ActorTargeting : MonoBehaviour
{
    public IReadOnlyCollection<GameObject> CurrentTargets => currentTargets;
    public IReadOnlyCollection<GameObject> EnteringTargets => enteringTargets;
    public IReadOnlyCollection<GameObject> ExitingTargets => exitingTargets;


    private readonly HashSet<GameObject> currentTargets = new();
    private readonly HashSet<GameObject> enteringTargets = new();
    private readonly HashSet<GameObject> exitingTargets = new();

    private readonly HashSet<GameObject> _lastFrameTargets = new();
    private readonly HashSet<GameObject> _thisFrameTargets = new();

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    GameObject ColliderToActor(Collider other)
    {
        GameObject rootObj = other.transform.root.gameObject;
        if (rootObj.layer == LayerMask.NameToLayer("Actors"))
            return rootObj;
        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject actor = ColliderToActor(other);
        if (actor != null)
            _thisFrameTargets.Add(actor);
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject actor = ColliderToActor(other);
        if (actor != null)
            _thisFrameTargets.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other) 
    {
        GameObject actor = ColliderToActor(other);
        if (actor != null)
            _thisFrameTargets.Remove(other.gameObject);
    }

    private void LateUpdate()
    {
        // Clear entering/exiting first
        enteringTargets.Clear();
        exitingTargets.Clear();

        // Calculate entering
        foreach (GameObject target in _thisFrameTargets)
            if (!_lastFrameTargets.Contains(target))
                enteringTargets.Add(target);
        
        // Calculate exiting
        foreach (GameObject target in _lastFrameTargets)
            if (!_thisFrameTargets.Contains(target))
                exitingTargets.Add(target);
        
        // Swap current state
        currentTargets.Clear();
        foreach (GameObject t in _thisFrameTargets)
            currentTargets.Add(t);

        // Prepare for next frame
        _lastFrameTargets.Clear();
        foreach (GameObject t in _thisFrameTargets)
            _lastFrameTargets.Add(t);

        // Reset next-frame accumulation
        _thisFrameTargets.Clear();
    }
}