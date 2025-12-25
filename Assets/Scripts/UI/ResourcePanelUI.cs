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
            CharacterResource trackedResource = trackedResources.GetResource(bar.LinkedResource.resourceType);

            trackedResource.MaxStat.OnValueChanged += HandleStatChanged;
            trackedResource.OnValueChanged += HandleResourceChanged;

            HandleStatChanged(trackedResource.MaxStat);
            HandleResourceChanged(trackedResource);
        }
    }

    void OnDestroy()
    {
        foreach (ResourceBarUI bar in bars)
        {
            CharacterResource trackedResource = trackedResources.GetResource(bar.LinkedResource.resourceType);
            trackedResource.OnValueChanged -= HandleResourceChanged;
            trackedResource.MaxStat.OnValueChanged -= HandleStatChanged;
        }
    }

    void HandleStatChanged(Stat stat) =>
        bars.FirstOrDefault(b => b.LinkedResource.MaxStat == stat.Definition).SetSliderMaxValue(stat.Value);

    void HandleResourceChanged(CharacterResource resource) =>
        bars.FirstOrDefault(b => b.LinkedResource == resource.Definition).SetSliderValue(resource.Value);
}
