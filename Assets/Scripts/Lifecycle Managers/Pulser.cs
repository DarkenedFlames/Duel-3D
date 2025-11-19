using UnityEngine;
using System;

public class Pulser : MonoBehaviour
{
    [Tooltip("The period in seconds at which the object pulses."), SerializeField]
    float period = 1f;

    Counter seconds;
    public event Action OnPulse;

    void Awake() => seconds = new Counter(period);
    
    void Update()
    {
        seconds.Decrease(Time.deltaTime);
        
        if (!seconds.Expired)
        {
            OnPulse?.Invoke();
            seconds.Reset();
        }
    }
}