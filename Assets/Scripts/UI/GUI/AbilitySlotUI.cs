using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilitySlotUI : MonoBehaviour
{
    public AbilityType abilityType;
    public Ability Ability;

    [SerializeField] Image icon;
    [SerializeField] Image overlay;
    [SerializeField] TextMeshProUGUI rankText;

    public void SetAbility(Ability ability)
    {
        if (ability == null) return;

        Ability = ability;
        icon.sprite = ability.Definition.icon;
    }

    public void UpdateRank()
    {
        if (Ability == null) return;

        rankText.text = GetNumeralForRank(Ability.Rank);
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

    static string GetNumeralForRank(int rank)
    {
        return rank switch
        {
            1 => "I",
            2 => "II",
            3 => "III",
            4 => "IV",
            5 => "V",
            _ => rank.ToString(),
        };
    }
}
