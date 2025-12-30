using UnityEngine;

[RequireComponent(typeof(IActionSource))]
public class MFollowOwner : MonoBehaviour
{
    [Tooltip("The local offset at which the object follows its owner."), SerializeField]
    Vector3 LocalOffset = Vector3.zero;

    void Update()
    {
        Character owner = GetComponent<IActionSource>().Owner;
        if (owner == null)
        {
            enabled = false;
            return;
        }

        transform.SetPositionAndRotation(owner.transform.TransformPoint(LocalOffset), owner.transform.rotation);
    }
}
