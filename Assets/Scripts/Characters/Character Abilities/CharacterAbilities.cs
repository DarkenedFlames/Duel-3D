using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[RequireComponent(typeof(Character))]
public class CharacterAbilities : MonoBehaviour
{
    [SerializeField] AbilityDefinitionSet startingAbilitySet;
    public readonly Dictionary<AbilityType, Ability> abilities = new();

    Character Owner { get; set; }

    public event Action<Ability> OnAbilityActivated;
    public event Action<Ability> OnAbilityLearned;
    public event Action<Ability> OnAbilityRankChanged;

    void Awake() 
    {
        Owner = GetComponent<Character>();
        foreach (AbilityDefinition definition in startingAbilitySet.definitions) 
            LearnAbility(definition);
    }
    void Update()
    {
        foreach (Ability ability in abilities.Values)
            ability.TickCooldown(Time.deltaTime);
    }
    void OnEnable() => Owner.CharacterInput.OnAbilityInput += TryActivateByType;
    void OnDisable() => Owner.CharacterInput.OnAbilityInput -= TryActivateByType;

    public bool TryGetAbility(AbilityDefinition definition, out Ability ability)
    {
        ability = abilities.Values.ToList().Find(a => a.Definition == definition);
        return ability != null;
    }

    public void LearnAbility(AbilityDefinition definition)
    {
        if (TryGetAbility(definition, out Ability existing))
        {
            if (existing.TryRankUp())
                OnAbilityRankChanged?.Invoke(existing);
        }
        else
        {
            Ability ability = new(Owner, definition);
            abilities[definition.abilityType] = ability;
            OnAbilityLearned?.Invoke(ability);
        }
    }
                
    void TryActivateByType(AbilityType type)
    {
        if(abilities.TryGetValue(type, out Ability ability) && ability.TryActivate())
            OnAbilityActivated?.Invoke(abilities[type]);
    }
}