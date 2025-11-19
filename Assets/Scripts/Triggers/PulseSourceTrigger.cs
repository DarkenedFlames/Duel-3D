using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Pulser), typeof(RequiresSource))]
public class PulseSourceTrigger : MonoBehaviour
{
    [Header("Actions")]
    [Tooltip("Actions to execute on this object's source upon pulsing."), SerializeReference]
    List<IGameAction> actions = new();

    Pulser pulser;
    GameObject source;

    void Awake()
    {
        source = GetComponent<RequiresSource>().Source;
        pulser = GetComponent<Pulser>();
    }

    void OnEnable() => pulser.OnPulse += HandlePulse;
    void OnDisable() => pulser.OnPulse -= HandlePulse;

    void HandlePulse()
    {
        if (source != null)
            foreach (IGameAction action in actions)
                action.Execute(source);
    }
}