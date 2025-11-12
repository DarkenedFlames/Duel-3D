using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Orbit Settings")]
    [SerializeField] private float distance = 6f;
    [SerializeField] private float sensitivityX = 1f;
    [SerializeField] private float sensitivityY = 0.5f;
    [SerializeField] private float minY = -30f;
    [SerializeField] private float maxY = 60f;

    private IInputProvider input;
    private float _yaw;
    private float _pitch;
    private Collider[] playerColliders;

    private void Awake()
    {
        input = target.GetComponent<IInputProvider>();
        playerColliders = target.GetComponentsInChildren<Collider>();
    }

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _yaw = angles.y;
        _pitch = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Should eventually be replaced with a proper layer mask system
    private bool RaycastWithoutPlayer(Vector3 start, Vector3 dir, out RaycastHit hit, float maxDist)
    {
        // Temporarily disable player colliders for this raycast
        foreach (var c in playerColliders) c.enabled = false;
        bool hasHit = Physics.Raycast(start, dir, out hit, maxDist);
        foreach (var c in playerColliders) c.enabled = true;
        return hasHit;
    }

    private void LateUpdate()
    {
        // Read camera input
        Vector2 look = input.LookInput;
        _yaw += look.x * sensitivityX;
        _pitch -= look.y * sensitivityY;
        _pitch = Mathf.Clamp(_pitch, minY, maxY);

        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distance);

        // Camera collision (keep this separate)
        if (RaycastWithoutPlayer(target.position, (desiredPosition - target.position).normalized, out RaycastHit hit, distance))
        {
            float targetDistance = Mathf.Clamp(hit.distance * 0.9f, 0.5f, distance);
            desiredPosition = target.position - (rotation * Vector3.forward * targetDistance);
        }

        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * 1f); // look at a point slightly above target for better framing

        // --- Rotate player horizontally to match camera yaw ---
        Vector3 forward = rotation * Vector3.forward;  // rotate forward by the quaternion
        forward.y = 0;                                // ignore vertical component
        forward.Normalize();                           // make it a proper direction

        if (forward.sqrMagnitude > 0.001f)
            target.rotation = Quaternion.LookRotation(forward);

    }
}
