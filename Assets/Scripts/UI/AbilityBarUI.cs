using System.Collections.Generic;
using UnityEngine;

public class AbilityBarUI : MonoBehaviour
{
    private AbilitySlotUI[] slots;
    private Dictionary<AbilityType, AbilitySlotUI> slotLookup;
    private CharacterAbilities abilityHandler;

    void Awake()
    {
        slots = GetComponentsInChildren<AbilitySlotUI>();
        BuildLookup();
    }

    public void SubscribeToHandler(CharacterAbilities handler)
    {
        abilityHandler = handler;
        abilityHandler.OnAbilityLearned += HandleAbilityLearned;
        abilityHandler.OnAbilityActivated += HandleAbilityUsed;

        foreach (Ability ability in abilityHandler.abilities.Values)
        {
            HandleAbilityLearned(ability);
        }
    }

    private void BuildLookup()
    {
        slotLookup = new Dictionary<AbilityType, AbilitySlotUI>();
        foreach (var slot in slots)
        {
            if (!slotLookup.ContainsKey(slot.abilityType))
                slotLookup.Add(slot.abilityType, slot);
            else
                Debug.LogWarning($"Duplicate ability slot for type {slot.abilityType}");
        }
    }

    private void HandleAbilityUsed(Ability ability)
    {
        if (slotLookup.TryGetValue(ability.Definition.abilityType, out AbilitySlotUI slot))
            slot.StartCooldown(ability.Definition.cooldown);
    }

    private void HandleAbilityLearned(Ability ability)
    {
        if (slotLookup.TryGetValue(ability.Definition.abilityType, out AbilitySlotUI slot))
            slot.SetIcon(ability.Definition.icon);
    }

    private void OnDestroy()
    {
        if (abilityHandler == null) return;
        abilityHandler.OnAbilityActivated -= HandleAbilityUsed;
        abilityHandler.OnAbilityLearned -= HandleAbilityLearned;
    }
}
