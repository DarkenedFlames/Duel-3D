using UnityEngine;
using TMPro;

public class GameFPSUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI fpsText;

    float deltaTime;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString() + " FPS";
    }
}