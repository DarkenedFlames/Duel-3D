using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class StatPanelUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject statSlotPrefab;
    [SerializeField] VerticalLayoutGroup layoutGroup;

    Character owner;

    readonly Dictionary<Stat, CharacterResource> maxStatToResource = new();

    readonly List<Stat> nonResourceStats = new();

    private void Awake()
    {
        if (layoutGroup == null)
            layoutGroup = GetComponent<VerticalLayoutGroup>();
    }

    public void SubscribeToHandler(Character character)
    {
        owner = character;

        foreach (CharacterResource resource in owner.CharacterResources.Resources)
        {
            Stat maxStat = owner.CharacterStats.GetStat(resource.Definition.MaxStat.statType);
            maxStatToResource[maxStat] = resource;
        }
        
        foreach (Stat stat in owner.CharacterStats.Stats)
            if (!maxStatToResource.ContainsKey(stat))
                nonResourceStats.Add(stat);

        
        foreach (var kvp in maxStatToResource)
        {
            GameObject spawnedUISlot = Instantiate(statSlotPrefab, layoutGroup.transform);
            if (!spawnedUISlot.TryGetComponent(out StatSlotUI uiComponent))
            {
                Debug.LogError("Stat Slot Prefab is missing StatSlotUI component!");
                Destroy(spawnedUISlot);
                continue;
            }

            uiComponent.Initialize(kvp.Key, kvp.Value);
        }

        foreach (Stat stat in nonResourceStats)
        {
            GameObject spawnedUISlot = Instantiate(statSlotPrefab, layoutGroup.transform);

            if (!spawnedUISlot.TryGetComponent(out StatSlotUI uiComponent))
            {
                Debug.LogError("Stat Slot Prefab is missing StatSlotUI component!");
                Destroy(spawnedUISlot);
                continue;
            }

            uiComponent.Initialize(stat);
        }

    }
}