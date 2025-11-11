// File: AbilityHandler.cs
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public enum AbilityAnimationTrigger { None, Cast, Channel }

/// <summary>
/// Attach to the player (or caster). Manages a set of learned ability instances and routes activation requests.
/// </summary>
[
    RequireComponent(typeof(StatsHandler)),
    RequireComponent(typeof(EffectHandler)),
    RequireComponent(typeof(AnimationHandler)),
    RequireComponent(typeof(StatusHandler))
]
public class AbilityHandler : MonoBehaviour
{
    [SerializeField] List<AbilityDefinition> initialAbilities;

    public readonly Dictionary<AbilityType, Ability> abilities = new();
    public HashSet<AbilityExecution> activeExecutions = new();

    public StatsHandler StatsHandler { get; private set; }
    public EffectHandler EffectHandler { get; private set; }
    public AnimationHandler AnimationHandler { get; private set; }
    public StatusHandler StatusHandler { get; private set; }

    public event Action<Ability, AbilityExecution> OnAbilityActivated;
    public event Action<Ability> OnAbilityLearned;

    void Awake()
    {
        StatsHandler = GetComponent<StatsHandler>();
        EffectHandler = GetComponent<EffectHandler>();
        AnimationHandler = GetComponent<AnimationHandler>();
        StatusHandler = GetComponent<StatusHandler>();
    }

    void Start()
    {
        foreach (AbilityDefinition definition in initialAbilities) 
            LearnAbility(definition);
    }

    void Update()
    {
        float dt = Time.deltaTime;       
        foreach (var ability in abilities.Values) ability.TickCooldown(dt);
        foreach (var exec in activeExecutions) exec.Tick(dt);
    }

    #region Ability Management

    public void LearnAbility(AbilityDefinition def)
    {
        Ability newAbility = new(def, this);
        abilities[def.abilityType] = newAbility;
        OnAbilityLearned?.Invoke(newAbility);
    }

    public bool TryGetAbility(AbilityType type, out Ability ability) => abilities.TryGetValue(type, out ability);

    #endregion
    
    #region Activation

    public bool TryActivateByType(AbilityType type, object ctx = null)
    {
        if (!TryGetAbility(type, out var ability))
            return false;

        return TryActivate(ability, ctx);
    }

    public bool TryActivate(Ability ability, object ctx = null)
    {
        if (!ability.IsReady) return false;

        var def = ability.Definition;

        foreach (var condition in def.activationConditions)
        {
            if (!condition.IsMet(this, ability)) return false;
        }

        StatsHandler.TryModifyStat(StatType.Mana, modifyMax: false, -1f * def.manaCost);
        ability.cooldownRemaining = def.cooldown;

        Vector3 aimDirection = Vector3.forward;
        var exec = new AbilityExecution(this, ability, aimDirection, ctx);
        exec.InitializeBehaviors(def.behaviorDefinitions);
        exec.Subscribe("OnEnd", _ => activeExecutions.Remove(exec));
        activeExecutions.Add(exec);

        if (def.castTime > 0f) StartCoroutine(CastAndActivate(exec, def.castTime));
        else exec.Activate();

        AnimationHandler.TriggerAbility(def.castAnimationTrigger.ToString());
        OnAbilityActivated?.Invoke(ability, exec);

        return true;
    }

    IEnumerator CastAndActivate(AbilityExecution exec, float castTime)
    {
        float t = 0f;
        while (t < castTime)
        {
            t += Time.deltaTime;
            yield return null;
        }
        exec.Activate();
    }

    #endregion
}