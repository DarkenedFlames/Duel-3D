using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EffectSlotUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Image icon;
    [SerializeField] Image overlay;
    [SerializeField] TextMeshProUGUI stackText;

    public CharacterEffect Effect;

    public void Initialize(CharacterEffect effect)
    {
        Effect = effect;
        icon.sprite = effect.Definition.icon;

        UpdateStackCount();
    }

    void Update()
    {
        if (Effect == null) return;

        overlay.fillAmount = Effect.seconds == null ? 0f : Effect.seconds.Progress;
    }
    
    public void UpdateStackCount()
    {
        if (Effect == null) return;

        stackText.text = Effect.currentStacks.Value > 1 
            ? Effect.currentStacks.Value.ToString()
            : string.Empty;
    }
}
