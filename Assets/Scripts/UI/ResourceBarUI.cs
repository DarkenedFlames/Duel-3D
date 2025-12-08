using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ResourceBarUI : MonoBehaviour
{
    public ResourceDefinition LinkedResource;
    public Slider Slider;

    void Awake() => Slider = GetComponent<Slider>();
    public void SetSliderValue(float value) => Slider.value = value;
    public void SetSliderMaxValue(float value) => Slider.maxValue = value;
}
