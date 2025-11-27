using UnityEngine;
public class MLob : MonoBehaviour
{
    [Header("Lob Settings")]
    [Tooltip("The speed (meters/second) at which the region moves.")]
    public float Speed = 10f;

    void Start()
    {
        if (Speed <= 0)
            Debug.LogError($"{name}'s Mover {nameof(MLob)} was configured with an invalid parameter: {nameof(Speed)} must be positive!");
    }

    void Update()
    {
        Vector3 velocity = transform.forward * Speed;
        velocity += Physics.gravity * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;
        transform.forward = velocity.normalized;
    }
}
