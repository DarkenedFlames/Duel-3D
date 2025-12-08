using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EffectSlotUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Image iconImage;
    [SerializeField] Image cooldownOverlay;
    [SerializeField] TextMeshProUGUI stackText;

    private CharacterEffect effect;

    public void Initialize(CharacterEffect effect)
    {
        this.effect = effect;
        iconImage.sprite = effect.Definition.icon;

        UpdateStackCount();
        UpdateDuration();
    }

    void Update() => UpdateDuration();
    
    public void UpdateStackCount()
    {
        if (effect == null) return;

        stackText.text = effect.currentStacks.Value > 1 
            ? effect.currentStacks.Value.ToString()
            : string.Empty;
    }

    public void UpdateDuration()
    {
        if (effect == null) return;

        cooldownOverlay.fillAmount = effect.seconds == null
            ? 0f
            : Mathf.Clamp01(effect.seconds.Value / effect.CurrentDuration);
    }

    public void PlayMaxStackEffect()
    {
    }

    public void PlayPulseEffect()
    {
    }
}
