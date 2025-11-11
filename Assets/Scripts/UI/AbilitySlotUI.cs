using UnityEngine;
using UnityEngine.UI;

public class AbilitySlotUI : MonoBehaviour
{
    public AbilityType abilityType;
    private Image icon;
    private Image cooldownOverlay;

    private float cooldownDuration;
    private float cooldownRemaining;
    private bool isCoolingDown = false;

    void Awake()
    {
        foreach (Image img in GetComponentsInChildren<Image>())
        {
            if (img.gameObject.name == "Icon")
                icon = img;
            if (img.gameObject.name == "CooldownOverlay")
                cooldownOverlay = img;
        }

        cooldownOverlay.fillAmount = 0f;
    }

    public void SetIcon(Sprite abilityIcon) => icon.sprite = abilityIcon;

    public void SetCooldownOverlay(Image cdOverlay) => cooldownOverlay = cdOverlay;

    public void StartCooldown(float duration)
    {
        if (cooldownOverlay == null) return;

        cooldownDuration = duration;
        cooldownRemaining = duration;
        isCoolingDown = true;
    }

    void Update()
    {
        if (!isCoolingDown) return;

        cooldownRemaining -= Time.deltaTime;
        cooldownOverlay.fillAmount = Mathf.Clamp01(cooldownRemaining / cooldownDuration);

        if (cooldownRemaining <= 0f)
        {
            cooldownOverlay.fillAmount = 0f;
            isCoolingDown = false;
        }
    }
}
