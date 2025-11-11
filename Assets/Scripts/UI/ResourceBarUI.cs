using UnityEngine;
using UnityEngine.UI;

public class ResourceBarUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider staminaBar;
    [SerializeField] private Slider manaBar;

    private StatsHandler trackedStats;

    public void Initialize(StatsHandler stats)
    {
        if (trackedStats != null)
            trackedStats.OnStatChanged -= HandleStatChanged;

        trackedStats = stats;
        trackedStats.OnStatChanged += HandleStatChanged;

        // Sync initial values
        healthBar.maxValue = stats.GetStat(StatType.Health, true);
        healthBar.value = stats.GetStat(StatType.Health, false);

        staminaBar.maxValue = stats.GetStat(StatType.Stamina, true);
        staminaBar.value = stats.GetStat(StatType.Stamina, false);

        manaBar.maxValue = stats.GetStat(StatType.Mana, true);
        manaBar.value = stats.GetStat(StatType.Mana, false);
    }

    private void HandleStatChanged(StatType type, float current, float max)
    {
        switch (type)
        {
            case StatType.Health:
                healthBar.maxValue = max;
                healthBar.value = current;
                break;

            case StatType.Stamina:
                staminaBar.maxValue = max;
                staminaBar.value = current;
                break;

            case StatType.Mana:
                manaBar.maxValue = max;
                manaBar.value = current;
                break;
        }
    }
}
