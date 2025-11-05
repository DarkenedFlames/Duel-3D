using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ResourceBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;

    void Awake()
    {
        if (slider == null) slider = GetComponent<Slider>();
    }

    public void SetValue(float current, float max)
    {
        if (max <= 0) return;
        slider.value = current / max;
    }
}
