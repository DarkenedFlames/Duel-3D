using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollisionLimiter : MonoBehaviour
{
    [Header("Collision Limiting Settings")]
    [Tooltip("The maximum number of collisions this object may experience before destruction."), SerializeField]
    int collisionLimit;

    // Must agree with Layer Collision Matrix
    [Tooltip("LayerMask to define which collisions count towards the limit."), SerializeField]
    LayerMask layerMask = LayerMask.NameToLayer("Actors");

    IntCounter collisions;

    bool LayerInMask(int layer, LayerMask mask) => (mask & (1 << layer)) != 0;
    void Awake() => collisions = new IntCounter(collisionLimit);
    void OnTriggerEnter(Collider other)
    {
        if (LayerInMask(other.gameObject.layer, layerMask)) collisions.Decrement();
    } 
    void Update()
    {
        if (collisions.Expired) Destroy(gameObject);
    }
}