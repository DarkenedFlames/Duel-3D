using UnityEngine;
using TMPro;

public class GameTimerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameSessionSettings settings;
    
    FloatCounter seconds;
    
    void Awake()
    {
        if (settings.Duration > 0)
            seconds = new(settings.Duration, 0, settings.Duration);
    }
    
    void Update()
    {
        if (seconds == null || seconds.Expired)
        {
            timerText.text = "00:00:00";
            return;
        }
        
        seconds.Decrease(Time.deltaTime);
        float t = seconds.Value;
        int c = 60;
        int hours   = Mathf.FloorToInt(t / (c * c));
        int minutes = Mathf.FloorToInt(t / c) - c * hours;
        int secs = Mathf.FloorToInt(t) - c * minutes - c * c * hours;
        
        timerText.text = $"{hours:D2}:{minutes:D2}:{secs:D2}";
    }
}