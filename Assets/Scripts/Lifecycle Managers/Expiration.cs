using System;
using UnityEngine;

public class Expiration : MonoBehaviour
{
    [Header("Expiration Settings")]
    [Tooltip("The number of seconds until the GameObject expires."), SerializeField]
    float duration = 10f;

    [Tooltip("If true, the GameObject will be automatically destroyed. If false, it will only invoke OnExpired.")]
    bool autoDestroy = false;

    Counter seconds;

    public event Action<Expiration> OnExpired;

    public void Reset() => seconds.Reset();
    public float Remaining() => seconds.Value;

    void Awake()
    {
        seconds = new Counter(duration);
    }

    void Update()
    {
        seconds.Decrease(Time.deltaTime);
        if (seconds.Expired)
        {
            OnExpired?.Invoke(this);
            if(autoDestroy)
                Destroy(gameObject);
        }
    }
}