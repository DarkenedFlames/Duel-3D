using UnityEngine;

[RequireComponent(typeof(SpawnContext))]
public class MFollowSource : MonoBehaviour
{
    [Tooltip("The local offset at which we follow the source.")]
    public Vector3 LocalOffset;

    void Update()
    {
        SpawnContext spawnContext = GetComponent<SpawnContext>();
        GameObject source = spawnContext.Owner != null ? spawnContext.Spawner : spawnContext.Owner;

        if (source == null) return;

        transform.SetPositionAndRotation(source.transform.TransformPoint(LocalOffset), source.transform.rotation);
    }
}
