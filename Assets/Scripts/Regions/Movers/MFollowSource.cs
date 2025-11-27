using UnityEngine;

[RequireComponent(typeof(SpawnContext))]
public class MFollowSource : MonoBehaviour
{
    [Tooltip("The local offset at which we follow the source.")]
    public Vector3 LocalOffset = Vector3.zero;

    void Update()
    {
        Character owner = GetComponent<SpawnContext>().Owner;
        if (owner == null)
        {
            Debug.LogError($"{name}'s Mover {nameof(MFollowSource)} found a null {nameof(SpawnContext.Owner)}");
            return;
        }

        transform.SetPositionAndRotation(owner.transform.TransformPoint(LocalOffset), owner.transform.rotation);
    }
}
