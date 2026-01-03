using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberIconUI : MonoBehaviour
{   
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

    public void Initialize(Sprite icon, string text = null, Color textColor = default)
    {
        seconds = new(0, 0, duration, resetToMax: false);
        mainCamera = Camera.main;
        Text.text = text ?? string.Empty;
        Image.sprite = icon;

        textStartingColor = textColor;
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
        float alpha = fadeCurve.Evaluate(seconds.Progress);
        Color imageColor = imageStartingColor;
        imageColor.a = alpha;
        Image.color = imageColor;

        if (Text.text != string.Empty)
        {
            Color textColor = textStartingColor;
            textColor.a = alpha;
            Text.color = textColor;
        }
        else
            Text.color = Color.clear;
    }
}
