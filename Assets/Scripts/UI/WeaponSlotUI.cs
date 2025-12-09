using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotUI : MonoBehaviour
{
    CharacterWeapons characterWeapons;
    private Image icon;
    private Image cooldownOverlay;

    FloatCounter seconds;

    void Awake()
    {
        seconds = new(0, 0, 0);

        foreach (Image img in GetComponentsInChildren<Image>())
        {
            if (img.gameObject.name == "Icon")
                icon = img;
            if (img.gameObject.name == "CooldownOverlay")
                cooldownOverlay = img;
        }

        cooldownOverlay.fillAmount = 0f;
    }

    public void SubscribeToHandler(CharacterWeapons characterWeapons)
    {
        this.characterWeapons = characterWeapons;
        characterWeapons.OnEquipWeapon += OnEquipWeapon;
        characterWeapons.OnWeaponUsed += OnWeaponUsed;

        if (characterWeapons.currentWeapon != null &&
            characterWeapons.currentWeapon.TryGetComponent(out Weapon weapon))
        {
            OnEquipWeapon(weapon);
        }
    }

    void OnDestroy()
    {
        characterWeapons.OnEquipWeapon -= OnEquipWeapon;
        characterWeapons.OnWeaponUsed -= OnWeaponUsed;
    }

    void OnEquipWeapon(Weapon weapon) => icon.sprite = weapon.Definition.Icon;
    void OnWeaponUsed(Weapon weapon) => StartCooldown(weapon.Definition.CooldownTime);

    public void StartCooldown(float duration)
    {
        seconds.SetMax(duration);
        seconds.Reset();
    }

    void Update()
    {
        if (seconds.Expired)
            cooldownOverlay.fillAmount = 0f;
        else
        {
            seconds.Decrease(Time.deltaTime);
            cooldownOverlay.fillAmount = Mathf.Clamp01(seconds.Value / seconds.Max);
        }
    }
}
