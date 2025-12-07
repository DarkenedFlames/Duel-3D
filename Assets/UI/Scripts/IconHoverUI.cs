using UnityEngine.UI;
using UnityEngine;

public class IconHoverUI : MonoBehaviour
{
    Vector3 offset = new(0, 2.5f, 0);
    Transform linkedTransform;

    void LateUpdate()
    {
        if (linkedTransform == null) return;

        transform.SetPositionAndRotation(
            linkedTransform.position + offset,
            Quaternion.LookRotation(transform.position - Camera.main.transform.position)
        );
    }

    public void Initialize(Transform targetTransform, Vector3 offset, Sprite sprite)
    {
        linkedTransform = targetTransform;
        this.offset = offset;

        GetComponent<Image>().sprite = sprite;
    }
}