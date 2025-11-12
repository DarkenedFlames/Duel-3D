using UnityEngine;
using UnityEngine.UI;

public class ResourceBarUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider staminaBar;
    [SerializeField] private Slider manaBar;

    private StatsHandler trackedStats;

    public void SubscribeToHandler(StatsHandler stats)
    {
        if (stats != null)
        {
            trackedStats = stats;
            trackedStats.OnStatChanged += HandleStatChanged;
        }
    }

    private void OnDestroy()
    {
        if (trackedStats != null)
            trackedStats.OnStatChanged -= HandleStatChanged;
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
