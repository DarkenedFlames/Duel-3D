using UnityEngine;
public class MStraightLine : MonoBehaviour
{
    [Header("Straight Line Settings")]
    [Tooltip("The speed (meters/second) at which the region moves.")]
    public float Speed = 10f;
    void Start()
    {
        if (Speed <= 0)
            Debug.LogError($"{name}'s Mover {nameof(MStraightLine)} was configured with an invalid parameter: {nameof(Speed)} must be positive!");
    }

    void Update() => transform.position += Speed * Time.deltaTime * transform.forward;
}