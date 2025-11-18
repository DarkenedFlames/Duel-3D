using UnityEngine;

public class Expiration : MonoBehaviour
{
    [Header("Expiration Settings")]
    [Tooltip("The number of seconds until the GameObject is destroyed."), SerializeField]
    float duration = 10f;

    Counter seconds;

    void Awake()
    {
        seconds = new Counter(duration);
    }

    void Update()
    {
        seconds.Decrease(Time.deltaTime);
        if (seconds.Expired)
            Destroy(gameObject);
    }
}