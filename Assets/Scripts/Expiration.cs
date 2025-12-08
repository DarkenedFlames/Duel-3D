using UnityEngine;

public class Expiration : MonoBehaviour
{
    [Header("Expiration Settings")]
    [Tooltip("Time in seconds before expiration."), SerializeField]
    float expirationTime = 5f;

    float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= expirationTime)
        {
            Destroy(gameObject);
        }
    }
}