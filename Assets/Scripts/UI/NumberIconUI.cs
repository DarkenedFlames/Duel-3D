using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberIconUI : MonoBehaviour
{
    public enum FormatMode
    {
        Flat,
        PercentAdd,
        PercentMult,
    }
    
    [Header("References")]
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] Image Image;
    
    [Header("Animation Settings")]
    [SerializeField] float duration = 1.5f;
    [SerializeField] AnimationCurve arcCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    [SerializeField] float horizontalRandomRange = 0.5f;
    [SerializeField] Vector3 totalArcOffset = new(0, 2f, 0);
    [SerializeField] Vector3 spawnOffset = new(0, 1f, 0);
    
    Vector3 startPosition;
    FloatCounter seconds;
    Camera mainCamera;
    Color imageStartingColor;
    Color textStartingColor;
    bool initialized = false;

    public void Initialize(float number, Color textColor, Sprite icon, FormatMode formatMode)
    {
        seconds = new(0, 0, duration, resetToMax: false);
        mainCamera = Camera.main;
        Text.text = FormatNumber(number, formatMode);
        Image.sprite = icon;

        textStartingColor = textColor;
        Text.color = textColor;

        imageStartingColor = Image.color;


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

        if (seconds.Exceeded)
        {
            Destroy(gameObject);
            return;
        }

        // Arc and face camera
        transform.SetPositionAndRotation(
            startPosition + (totalArcOffset * arcCurve.Evaluate(seconds.Progress)), 
            Quaternion.LookRotation(transform.position - mainCamera.transform.position)
        );

        // Fade out
        Color textColor = textStartingColor;
        Color imageColor = imageStartingColor;
        
        float alpha = fadeCurve.Evaluate(seconds.Progress);
        textColor.a = alpha;
        imageColor.a = alpha;
        
        Text.color = textColor;
        Image.color = imageColor;
    }
    
    string FormatNumber(float number, FormatMode formatMode)
    {
        string sign;
        string displayNumber;
        string suffix = "";
        switch (formatMode)
        {
            case FormatMode.Flat:
                sign = number >= 0 ? "+" : "-";
                displayNumber = Mathf.Round(Mathf.Abs(number)).ToString();
                break;
            case FormatMode.PercentAdd:
                sign = number >= 0 ? "+" : "-";
                displayNumber = (number * 100).ToString("F2");
                suffix = "%";
                break;
            case FormatMode.PercentMult:
                sign = "x";
                displayNumber = Mathf.Abs(number).ToString("F2");
                break;
            default:
                sign = "?";
                displayNumber = "?";
                break;
        }
        
        return $"{sign}{displayNumber}{suffix}"; 
    }
}
