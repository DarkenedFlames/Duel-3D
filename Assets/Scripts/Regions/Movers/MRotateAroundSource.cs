using UnityEngine;
using System;

[Serializable]
[RequireComponent(typeof(SpawnContext))]
public class MRotateAroundSource : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("The rate (degrees/second) at which the object rotates around its owner.")]
    public float ArcDegreesPerSecond = 90f;

    [Tooltip("The distance (meters) at which the object rotates.")]
    public float Radius = 3f;

    [Tooltip("The vertical offset (meters) at which the object rotates.")]
    public float VerticalOffset = 1f;

    [Tooltip("Whether the object rotates clockwise.")]
    public bool Clockwise = true;

    float currentAngle;
    Character owner;

    void Start()
    {
        owner = GetComponent<SpawnContext>().Owner;
        if (owner == null)
        {
            Debug.LogError($"{name}'s Mover {nameof(MRotateAroundSource)} found a null {nameof(SpawnContext.Owner)}");
            return;
        }
        if (ArcDegreesPerSecond <= 0)
            Debug.LogError($"{name}'s Mover {nameof(MRotateAroundSource)} was configured with an invalid parameter: {nameof(ArcDegreesPerSecond)} must be positive!");
        if (Radius <= 0)
            Debug.LogError($"{name}'s Mover {nameof(MRotateAroundSource)} was configured with an invalid parameter: {nameof(Radius)} must be positive!");        

        Vector3 delta = transform.position - owner.transform.position;
        Vector2 flat = new(delta.x, delta.z);

        currentAngle = flat.sqrMagnitude > 0.001f
            ? Mathf.Atan2(flat.y, flat.x)
            : 0f;
    }

    void Update()
    {
        if (owner == null) return;

        float direction = Clockwise ? -1f : 1f;
        currentAngle += direction * ArcDegreesPerSecond * Mathf.Deg2Rad * Time.deltaTime;

        Vector3 horizontalOffset =
            new Vector3(Mathf.Cos(currentAngle), 0f, Mathf.Sin(currentAngle)) * Radius;

        Vector3 newPos = owner.transform.position + horizontalOffset;
        newPos.y += VerticalOffset;

        transform.position = newPos;

        Vector3 flatDirection = transform.position - owner.transform.position;
        flatDirection.y = 0f;

        if (flatDirection.sqrMagnitude > 0.001f)
            transform.forward = flatDirection.normalized;
    }
}

