using UnityEngine;
using System;
using System.Collections.Generic;

public class CollisionCollector : MonoBehaviour
{
    public event Action<GameObject> OnActorEnter;
    public readonly HashSet<Collider> currentTargets = new();

    void OnTriggerEnter(Collider other) => currentTargets.Add(other);
    void OnTriggerExit(Collider other) => currentTargets.Remove(other);
}
