using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Pulser), typeof(Collider))]
public class PulsePhysicsTrigger : MonoBehaviour
{
    [Header("Actions")]
    [Tooltip("Actions to execute on objects colliding with this object upon pulsing."), SerializeReference]
    List<IGameAction> actions = new();

    Pulser pulser;
    readonly HashSet<Collider> colliders = new();

    void Awake() => pulser = GetComponent<Pulser>();
    void OnEnable() => pulser.OnPulse += HandlePulse;
    void OnDisable() => pulser.OnPulse -= HandlePulse;
    void OnTriggerEnter(Collider other) => colliders.Add(other);
    void OnTriggerExit(Collider other) => colliders.Remove(other);

    void HandlePulse()
    {
        foreach (Collider collider in colliders)
            foreach (IGameAction action in actions)
                action.Execute(collider.gameObject);
    }
}