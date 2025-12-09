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
    [SerializeField] float horizontalRandomRange = 0.5f;
    [SerializeField] Vector3 totalArcOffset = new(0, 2f, 0);
    [SerializeField] Vector3 spawnOffset = new(0, 1f, 0);

    private Vector3 startPosition;
    private FloatCounter seconds;
    private Camera mainCamera;
    private Color startingColor;

    private bool initialized = false;

    public void Initialize(float damage, Color color)
    {
        seconds = new(0, 0, duration, resetToMax: false);
        mainCamera = Camera.main;
        damageText.text = Mathf.RoundToInt(damage).ToString();
        startingColor = color;
        
        Vector3 randomOffset = new(
            Random.Range(-horizontalRandomRange, horizontalRandomRange),
            0,
            Random.Range(-horizontalRandomRange, horizontalRandomRange)
        );
        transform.position += spawnOffset + randomOffset;
        startPosition = transform.position;

        initialized = true;
    }

    void Update()
    {
        if (!initialized)
            return;

        seconds.Increase(Time.deltaTime);
        float progress = seconds.Value / seconds.Max;

        if (progress >= 1f)
        {
            Destroy(gameObject);
            return;
        }

        // Arc and face camera
        float arcAmount = arcCurve.Evaluate(progress);
        transform.SetPositionAndRotation(
            startPosition + (totalArcOffset * arcAmount), 
            Quaternion.LookRotation(transform.position - mainCamera.transform.position)
        );

        // Fade out
        Color textColor = startingColor;
        textColor.a = fadeCurve.Evaluate(progress);
        damageText.color = textColor;
    }
}
