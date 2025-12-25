using UnityEngine;
using UnityEngine.UI;

public class AbilitySlotUI : MonoBehaviour
{
    public AbilityType abilityType;
    public Ability Ability;

    [SerializeField] Image icon;
    [SerializeField] Image overlay;

    public void SetAbility(Ability ability)
    {
        if (ability == null) return;

        Ability = ability;
        icon.sprite = ability.Definition.icon;
    }

    void Update()
    {
        if (Ability == null) return;

        if (Ability.seconds.Expired)
        {
            overlay.fillAmount = 0f;
            return;
        }

        overlay.fillAmount = Ability.seconds.Progress;
    }
}
