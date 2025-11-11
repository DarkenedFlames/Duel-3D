using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EffectSlotUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private TextMeshProUGUI stackText;

    private Effect effect;

    public void Initialize(Effect effect)
    {
        this.effect = effect;

        if (iconImage != null)
            iconImage.sprite = effect.Definition.icon;

        UpdateStackCount(effect.CurrentStacks);
        RefreshDuration(effect.RemainingTime(), effect.Definition.duration);
    }

    private void Update()
    {
        if (effect == null) return;
        RefreshDuration(effect.RemainingTime(), effect.Definition.duration);
    }

    public void UpdateStackCount(int stacks)
    {
        if (stackText != null)
            stackText.text = stacks > 1 ? stacks.ToString() : string.Empty;
    }

    public void RefreshDuration(float remaining, float total)
    {
        if (cooldownOverlay == null) return;
        cooldownOverlay.fillAmount = Mathf.Clamp01(remaining / total);
    }
}
