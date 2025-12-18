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
        if (allCharacters == null || !allCharacters.TryGetSinglePlayer(out Character localPlayer))
        {
            Debug.LogError("No player found for Camera initialization.");
            Debug.Log($"Character Count: {allCharacters.ToList().Count}");
            return;
        }
        
        target = localPlayer;
        input = target.CharacterInput;


        Vector3 angles = transform.eulerAngles;
        _pitch = angles.x;
        if (_pitch > 180f) _pitch -= 360f;
        _pitch = Mathf.Clamp(_pitch, minY, maxY);

        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (input == null || target == null) return;
        Transform targetTransform = target.transform;

        _pitch -= input.LookInput.y * sensitivityY;
        _pitch = Mathf.Clamp(_pitch, minY, maxY);

        Quaternion rotation = Quaternion.Euler(_pitch, targetTransform.eulerAngles.y, 0f);
        Vector3 desiredPosition = targetTransform.position - rotation * Vector3.forward * distance;
        Vector3 direction = (desiredPosition - targetTransform.position).normalized;

        if (Physics.Raycast(targetTransform.position, direction, out RaycastHit hit, distance, ~layerMask))
            desiredPosition = targetTransform.position - rotation * Vector3.forward * hit.distance * 0.9f;
        
        transform.position = desiredPosition;
        transform.LookAt(targetTransform.position + Vector3.up);
    }
}
