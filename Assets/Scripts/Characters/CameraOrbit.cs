using System.Linq;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Header("Orbit Settings")]
    [Tooltip("The global character set."), SerializeField]
    CharacterSet allCharacters;

    [Tooltip("The layers that the camera does not collide with."), SerializeField]
    LayerMask layerMask;

    [Tooltip("The distance the camera follows at."), SerializeField]
    float distance = 8f;

    [Tooltip("The sensitivity of changing pitch."), SerializeField]
    float sensitivityY = 5f;

    [Tooltip("The minimum pitch angle (degrees)."), SerializeField]
    float minY = -30f;

    [Tooltip("The maximum pitch angle (degrees)."), SerializeField]
    float maxY = 60f;

    float _pitch;

    IInputDriver input;
    Character target;

    void Start()
    {
        if (allCharacters == null || !allCharacters.TryGetSinglePlayer(out target))
            return;

        input = target.CharacterInput;

        _pitch = Mathf.Clamp(transform.eulerAngles.x, minY, maxY);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (input == null || target == null) return;
        Transform targetTransform = target.transform;

        _pitch -= input.LookInput.y * sensitivityY;
        _pitch = Mathf.Clamp(_pitch, minY, maxY);

        Quaternion rotation = Quaternion.Euler(_pitch, targetTransform.eulerAngles.y, 0f);
        Vector3 direction = rotation * Vector3.back * distance;

        transform.position = Physics.Raycast(targetTransform.position, direction.normalized, out RaycastHit hit, distance, ~layerMask)
            ? targetTransform.position + rotation * Vector3.back * hit.distance * 0.9f
            : targetTransform.position + direction;

        transform.LookAt(targetTransform.position + Vector3.up);
    }
}
