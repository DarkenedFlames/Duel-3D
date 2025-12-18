using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatSlotUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI statNameText;
    [SerializeField] TextMeshProUGUI statValueText;
    [SerializeField] Image statIconImage;

    Stat trackedStat;
    CharacterResource trackedResource;

    public void Initialize(Stat stat, CharacterResource resource = null)
    {
        trackedStat = stat;
        trackedResource = resource;

        statNameText.text = trackedResource?.Definition.ResourceName ?? trackedStat.Definition.statName;
        statIconImage.sprite = trackedResource?.Definition.Icon ?? trackedStat.Definition.Icon;

        UpdateText();
        trackedStat.OnValueChanged += OnValueChanged;
        if (trackedResource != null)
            trackedResource.OnValueChanged += OnValueChanged;
    }

    void UpdateText()
    {
        if (trackedResource != null)
            statValueText.text = $"{trackedResource.Value:F0} / {trackedStat.Value:F0}";
        else
            statValueText.text = $"{trackedStat.Value:F0}";
    }

    void OnValueChanged(Stat _) => UpdateText();
    void OnValueChanged(CharacterResource _) => UpdateText();

    void OnDestroy()
    {
        trackedStat.OnValueChanged -= OnValueChanged;
        if (trackedResource != null)
            trackedResource.OnValueChanged -= OnValueChanged;
    }
}