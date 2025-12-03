using UnityEngine;
public class MStraightLine : MonoBehaviour
{
    [Header("Straight Line Settings")]
    [Tooltip("The speed (meters/second) at which the region moves."), SerializeField, Min(0)]
    float Speed = 10f;
    void Update() => transform.position += Speed * Time.deltaTime * transform.forward;
}