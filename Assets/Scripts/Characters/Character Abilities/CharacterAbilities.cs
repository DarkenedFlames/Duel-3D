using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[RequireComponent(typeof(PlayerInputDriver))]
public class CharacterAbilities : MonoBehaviour
{
    [SerializeField] List<AbilityDefinition> initialAbilities;
    public readonly Dictionary<AbilityType, Ability> abilities = new();

    private PlayerInputDriver input;

    public event Action<Ability> OnAbilityActivated;
    public event Action<Ability> OnAbilityLearned;

    void Awake()
    {
        input = GetComponent<PlayerInputDriver>();
        input.OnAbilityInput += TryActivateByType;
    }

    void Start()
    {
        foreach (AbilityDefinition definition in initialAbilities)
            LearnAbility(definition);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        abilities.Values.ToList().ForEach(a => a.TickCooldown(dt));
    }

    void OnDestroy() => input.OnAbilityInput -= TryActivateByType;

    public void LearnAbility(AbilityDefinition def)
    {
        Ability newAbility = new(def, this);
        abilities[def.abilityType] = newAbility;
        OnAbilityLearned?.Invoke(newAbility);
    }

    void TryActivateByType(AbilityType type)
    {
        if(abilities[type].TryActivate())
            OnAbilityActivated?.Invoke(abilities[type]);
    }
}