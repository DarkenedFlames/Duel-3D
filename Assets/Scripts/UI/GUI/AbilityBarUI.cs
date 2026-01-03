using UnityEngine;

public class AbilityBarUI : MonoBehaviour
{
    AbilitySlotUI[] slots;
    CharacterAbilities abilities;

    void Awake() => slots = GetComponentsInChildren<AbilitySlotUI>();
    
    bool TryGetSlotByAbility(Ability ability, out AbilitySlotUI slot)
    {
        slot = System.Array.Find(slots, s => s.abilityType == ability.Definition.abilityType);
        if (slot == null)
            Debug.LogError("No slot found for ability: " + ability.Definition.name);

        return slot != null;
    }

    public void Initialize(Character owner)
    {
        abilities = owner.CharacterAbilities;
        abilities.OnAbilityLearned += OnAbilityLearned;
        abilities.OnAbilityRankChanged += OnAbilityRankChanged;

        foreach (AbilitySlotUI slot in slots)
            if (abilities.abilities.TryGetValue(slot.abilityType, out Ability ability))
            {
                slot.SetAbility(ability);
                slot.UpdateRank();
            }
    }

    void OnAbilityLearned(Ability ability)
    {
        if (TryGetSlotByAbility(ability, out AbilitySlotUI slot))
        {
            slot.SetAbility(ability);
            slot.UpdateRank();
        }
    }

    void OnAbilityRankChanged(Ability ability)
    {
        if (TryGetSlotByAbility(ability, out AbilitySlotUI slot))
            slot.UpdateRank();
    }

    void OnDestroy() 
    {
        abilities.OnAbilityLearned -= OnAbilityLearned;
        abilities.OnAbilityRankChanged -= OnAbilityRankChanged;
    }
    
}
