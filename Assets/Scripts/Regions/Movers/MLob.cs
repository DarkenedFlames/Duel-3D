using UnityEngine;
public class MLob : MonoBehaviour
{
    [Header("Lob Settings")]
    [Tooltip("The speed (meters/second) at which the region moves."), SerializeField, Min(0)]
    float Speed = 10f;

    void Update()
    {
        Vector3 velocity = transform.forward * Speed;
        velocity += Physics.gravity * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;
        transform.forward = velocity.normalized;
    }
}
