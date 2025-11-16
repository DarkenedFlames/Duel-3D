using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using System.Linq;

public enum AbilityAnimationTrigger { None, Cast, Channel }

public class AbilityHandler : MonoBehaviour, IHasSourceActor
{
    [SerializeField] List<AbilityDefinition> initialAbilities;
    public readonly Dictionary<AbilityType, Ability> abilities = new();

    public GameObject SourceActor { get; set; }

    private IInputProvider input;

    public event Action<Ability> OnAbilityActivated;
    public event Action<Ability> OnAbilityLearned;

    void Awake()
    {
        input = GetComponent<IInputProvider>();
        SourceActor = gameObject;
    }


    void Start()
    {
        foreach (AbilityDefinition definition in initialAbilities)
            LearnAbility(definition);
    }

    void Update()
    {
        if (input.AbilityPressed[0]) TryActivate(abilities[AbilityType.Primary]);
        if (input.AbilityPressed[1]) TryActivate(abilities[AbilityType.Secondary]);
        if (input.AbilityPressed[2]) TryActivate(abilities[AbilityType.Utility]);
        if (input.AbilityPressed[3]) TryActivate(abilities[AbilityType.Special]);

        float dt = Time.deltaTime;
        abilities.Values.ToList().ForEach(a => a.TickCooldown(dt));
        // figure out best way to tick executions... don't need it yet
    }

    public void LearnAbility(AbilityDefinition def)
    {
        Ability newAbility = new(def, this);
        abilities[def.abilityType] = newAbility;
        OnAbilityLearned?.Invoke(newAbility);
    }

    void TryActivate(Ability ability)
    {
        var def = ability.Definition;

        foreach (ActivationCondition condition in def.activationConditions)
            if (!condition.IsMet(ability)) return;

        if (!ability.ResetCooldown()) return;

        AbilityExecution execution = new(ability);

        if (def.castTime > 0f) StartCoroutine(CastAndActivate(execution, def.castTime));
        else execution.Activate();            

        OnAbilityActivated?.Invoke(ability);
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
}