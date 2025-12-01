using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CharacterAbilities : MonoBehaviour
{
    [SerializeField] List<AbilityDefinition> initialAbilities;
    public readonly Dictionary<AbilityType, Ability> abilities = new();

    IInputDriver input;

    public event Action<Ability> OnAbilityActivated;
    public event Action<Ability> OnAbilityLearned;

    void Awake()
    {
        if (!TryGetComponent(out IInputDriver inputDriver))
            Debug.LogError($"{name}'s {nameof(CharacterAbilities)} expected a component but it was missing: {nameof(IInputDriver)} missing!");
        else input = inputDriver;
    }

    void Start() { foreach (AbilityDefinition definition in initialAbilities) LearnAbility(definition); }
    void Update() => abilities.Values.ToList().ForEach(a => a.TickCooldown(Time.deltaTime));
    void OnEnable() => input.OnAbilityInput += TryActivateByType;
    void OnDisable() => input.OnAbilityInput -= TryActivateByType;

    public void LearnAbility(AbilityDefinition def)
    {
        Ability newAbility = new(GetComponent<Character>(), def);
        abilities[def.abilityType] = newAbility;
        OnAbilityLearned?.Invoke(newAbility);
    }

    void TryActivateByType(AbilityType type)
    {
        if(abilities.TryGetValue(type, out Ability ability) && ability.TryActivate())
            OnAbilityActivated?.Invoke(abilities[type]);
    }
}