using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Header("Orbit Settings")]
    [SerializeField] Transform target;
    [SerializeField] float distance = 8f;
    [SerializeField] float sensitivityY = 5f;
    [SerializeField] float minY = -30f;
    [SerializeField] float maxY = 60f;

    float _pitch;

    IInputDriver input;

    void Awake()
    {
        if (!target.TryGetComponent(out IInputDriver inputDriver))
            Debug.LogError($"{target.name}'s {nameof(CameraOrbit)} expected a component but it was missing: {nameof(IInputDriver)} missing!");
        else input = inputDriver;
    }

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _pitch = angles.x;
        if (_pitch > 180f) _pitch -= 360f;
        _pitch = Mathf.Clamp(_pitch, minY, maxY);

        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (input == null || target == null) return;

        _pitch -= input.LookInput.y * sensitivityY;
        _pitch = Mathf.Clamp(_pitch, minY, maxY);

        Quaternion rotation = Quaternion.Euler(_pitch, target.eulerAngles.y, 0f);
        Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance;
        Vector3 direction = (desiredPosition - target.position).normalized;

        if (Physics.Raycast(target.position, direction, out RaycastHit hit, distance, ~LayerMask.GetMask("Characters")))
            desiredPosition = target.position - rotation * Vector3.forward * hit.distance * 0.9f;
        
        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up);
    }
}
