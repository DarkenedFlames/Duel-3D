using System.Linq;
using UnityEngine;

public class ResourcePanelUI : MonoBehaviour
{
    ResourceBarUI[] bars;
    private CharacterStats trackedStats;

    void Awake()
    {
        bars = GetComponentsInChildren<ResourceBarUI>();
    }

    public void SubscribeToHandler(CharacterStats stats)
    {
        trackedStats = stats;
        if (trackedStats == null) return;

        foreach (ResourceBarUI bar in bars)
        {
            if (trackedStats.TryGetStat(bar.StatName, out ClampedStat trackedStat))
            {
                trackedStat.MaxStat.OnValueChanged += HandleStatChanged;
                trackedStat.OnValueChanged += HandleStatChanged;

                HandleStatChanged(trackedStat.MaxStat);
                HandleStatChanged(trackedStat);
            }
            else
                Debug.LogError($"{name} couldn't find stat {bar.StatName} provided by {bar.name} for UI subscription");
        }
    }

    void OnDestroy()
    {
        if (trackedStats == null) return;

        foreach (ResourceBarUI bar in bars)
        {
            if (trackedStats.TryGetStat(bar.StatName, out ClampedStat trackedStat))
            {
                trackedStat.OnValueChanged -= HandleStatChanged;
                trackedStat.MaxStat.OnValueChanged -= HandleStatChanged;
            }
            else
                Debug.LogError($"{name} couldn't find stat {bar.StatName} provided by {bar.name} for UI unsubscription");
        }
    }

    void HandleStatChanged(Stat stat)
    {
        ResourceBarUI bar = bars.FirstOrDefault(b => b.StatName == stat.Definition.statName);

        if(stat is ClampedStat)
            bar.SetSliderValue(stat.Value);
        else
            bar.SetSliderMaxValue(stat.Value);
    }
}
