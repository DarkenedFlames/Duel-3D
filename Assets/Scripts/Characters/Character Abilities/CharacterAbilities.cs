using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[RequireComponent(typeof(Character))]
public class CharacterAbilities : MonoBehaviour
{
    [SerializeField] AbilityDefinitionSet startingAbilitySet;
    public readonly Dictionary<AbilityType, Ability> abilities = new();

    Character Owner => GetComponent<Character>();

    public event Action<Ability> OnAbilityActivated;
    public event Action<Ability> OnAbilityLearned;

    void Start() 
    { 
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

    public void LearnAbility(AbilityDefinition def)
    {
        Ability newAbility = new(Owner, def);
        abilities[def.abilityType] = newAbility;
        OnAbilityLearned?.Invoke(newAbility);
    }

    public bool HasAbility(AbilityDefinition definition) => abilities.Values.Any(a => a.Definition == definition);
            
    void TryActivateByType(AbilityType type)
    {
        if(abilities.TryGetValue(type, out Ability ability) && ability.TryActivate())
            OnAbilityActivated?.Invoke(abilities[type]);
    }
}