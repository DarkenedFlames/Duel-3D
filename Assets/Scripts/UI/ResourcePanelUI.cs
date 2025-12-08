using System.Linq;
using UnityEngine;

public class ResourcePanelUI : MonoBehaviour
{
    ResourceBarUI[] bars;
    private CharacterResources trackedResources;

    void Awake() => bars = GetComponentsInChildren<ResourceBarUI>();
    public void SubscribeToHandler(CharacterResources resources)
    {
        trackedResources = resources;
        if (trackedResources == null) return;

        foreach (ResourceBarUI bar in bars)
        {
            if (trackedResources.TryGetResource(bar.LinkedResource, out CharacterResource trackedResource))
            {
                trackedResource.MaxStat.OnValueChanged += HandleStatChanged;
                trackedResource.OnValueChanged += HandleResourceChanged;

                HandleStatChanged(trackedResource.MaxStat);
                HandleResourceChanged(trackedResource);
            }
            else
                Debug.LogError($"{name} couldn't find stat {bar.LinkedResource.ResourceName} provided by {bar.name} for UI subscription");
        }
    }

    void OnDestroy()
    {
        if (trackedResources == null) return;

        foreach (ResourceBarUI bar in bars)
        {
            if (trackedResources.TryGetResource(bar.LinkedResource, out CharacterResource trackedResource))
            {
                trackedResource.OnValueChanged -= HandleResourceChanged;
                trackedResource.MaxStat.OnValueChanged -= HandleStatChanged;
            }
            else
                Debug.LogError($"{name} couldn't find stat {bar.LinkedResource.ResourceName} provided by {bar.name} for UI unsubscription");
        }
    }

    void HandleStatChanged(Stat stat)
    {
        ResourceBarUI bar = bars.FirstOrDefault(b => b.LinkedResource.MaxStat == stat.Definition);
        bar.SetSliderMaxValue(stat.Value);
    }

    void HandleResourceChanged(CharacterResource resource)
    {
        ResourceBarUI bar = bars.FirstOrDefault(b => b.LinkedResource == resource.Definition);
        bar.SetSliderValue(resource.Value);
    }
}
