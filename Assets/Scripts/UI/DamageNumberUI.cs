using UnityEngine;
using TMPro;

/// <summary>
/// Displays a damage number that floats up and fades out
/// Attach this to a world-space Canvas element
/// </summary>
public class DamageNumberUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI damageText;

    [Header("Animation Settings")]
    [SerializeField] float duration = 1.5f;
    [SerializeField] AnimationCurve arcCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    [SerializeField] Color spawnColor = Color.red;
    [SerializeField] float horizontalRandomRange = 0.5f;
    [SerializeField] Vector3 totalArcOffset = new(0, 2f, 0);
    [SerializeField] Vector3 spawnOffset = new(0, 1f, 0);

    private Vector3 startPosition;
    private FloatCounter seconds;
    private Camera mainCamera;

    void Awake()
    {
        seconds = new(0, 0, duration, resetToMax: false);
        mainCamera = Camera.main;
        
        Vector3 randomOffset = new(
            Random.Range(-horizontalRandomRange, horizontalRandomRange),
            0,
            Random.Range(-horizontalRandomRange, horizontalRandomRange)
        );
        transform.position += spawnOffset + randomOffset;
        startPosition = transform.position;
    }

    void Update()
    {
        seconds.Increase(Time.deltaTime);
        float progress = seconds.Value / duration;

        if (progress >= 1f)
        {
            Destroy(gameObject);
            return;
        }

        float arcAmount = arcCurve.Evaluate(progress);
        transform.position = startPosition + (totalArcOffset * arcAmount);
        
        if (mainCamera != null)
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);

        // Fade out
        Color textColor = spawnColor;
        textColor.a = fadeCurve.Evaluate(progress);
        damageText.color = textColor;
    }

    public void Initialize(float damage)
    {
        damageText.text = Mathf.RoundToInt(damage).ToString();
        damageText.color = spawnColor;
        seconds.Reset();
    }
}
