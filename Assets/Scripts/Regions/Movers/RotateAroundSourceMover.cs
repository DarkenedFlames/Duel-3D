using UnityEngine;
using System;

[Serializable]
public class RotateAroundSourceMover : IRegionMover
{
    public float ArcDegreesPerSecond = 90f;
    public float Radius = 3f;
    public float VerticalOffset = 1f;
    public bool Clockwise = true;

    private float currentAngle;
    private bool initialized;

    public IRegionMover Clone()
    {
        RotateAroundSourceMover copy = (RotateAroundSourceMover)MemberwiseClone();
        copy.currentAngle = 0;
        copy.initialized = false;
        return copy;
    }

    public void Tick(Region region)
    {
        SpawnContext ctx = region.GetComponent<SpawnContext>();
        GameObject source = ctx.Spawner != null ? ctx.Spawner : ctx.Owner;
        if (source == null) return;

        if (!initialized)
        {
            Vector3 delta = region.transform.position - source.transform.position;
            Vector2 flat = new(delta.x, delta.z);

            currentAngle = flat.sqrMagnitude > 0.001f
                ? Mathf.Atan2(flat.y, flat.x)
                : 0f;

            initialized = true;
        }

        float direction = Clockwise ? -1f : 1f;
        currentAngle += direction * ArcDegreesPerSecond * Mathf.Deg2Rad * Time.deltaTime;

        // --- Correct circular offset (XZ only) ---
        Vector3 horizontalOffset =
            new Vector3(Mathf.Cos(currentAngle), 0f, Mathf.Sin(currentAngle)) * Radius;

        // --- Apply vertical offset separately ---
        Vector3 newPos = source.transform.position + horizontalOffset;
        newPos.y += VerticalOffset;

        region.transform.position = newPos;

        // --- Forward direction stays horizontal (no tilt) ---
        Vector3 flatDirection = region.transform.position - source.transform.position;
        flatDirection.y = 0f;

        if (flatDirection.sqrMagnitude > 0.001f)
            region.transform.forward = flatDirection.normalized;
    }

}

