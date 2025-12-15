using System.Collections.Generic;
using UnityEngine;

public enum DamageType { Physical, Magical, True }

[System.Serializable]
public class ADealsDamage : IGameAction
{
    [Header("Conditions")]
    [SerializeReference]
    public List<IActionCondition> Conditions;

    [Header("Target Configuration")]
    [Tooltip("Who to damage: Owner (caster/summoner) or Target (hit character)."), SerializeField]
    ActionTargetMode targetMode = ActionTargetMode.Target;

    [Header("Damage Configuration")]
    [Tooltip("Whether to reset regeneration if damage is dealt."), SerializeField]
    bool resetRegenerationIfChanged = true;

    [Tooltip("The amount of damage dealt."), SerializeField, Min(0)]
    float amount = 1f;

    [Tooltip("The type of damage dealt."), SerializeField]
    DamageType damageType = DamageType.Physical;


    [Header("Damage Effects")]
    [SerializeField] GameObject damageNumberPrefab;
    [SerializeField] GameObject damageParticlesPrefab;

    public void Execute(ActionContext context)
    {
        Character target = targetMode switch
        {
            ActionTargetMode.Owner => context.Source.Owner,
            ActionTargetMode.Target => context.Target,
            _ => null,
        };

        if (target == null)
        {
            Debug.LogWarning($"{nameof(ADealsDamage)}: {targetMode} is null. Action skipped.");
            return;
        }

        if (Conditions != null)
        {
            foreach (IActionCondition condition in Conditions)
                if (!condition.IsSatisfied(context))
                    return;
        }
        
        if (Mathf.Approximately(0, amount)) return;

        float adjustedAmount = amount;

        // Owner adjustment

        Character owner = context.Source.Owner;
        if (owner != null)
        {
            CharacterStats ownerStats = owner.CharacterStats;
            Stat ownerAttack = ownerStats.GetStat(StatType.Attack, this);
            Stat ownerCriticalChance = ownerStats.GetStat(StatType.CriticalChance, this);
            Stat ownerCriticalDamage = ownerStats.GetStat(StatType.CriticalDamage, this);

            adjustedAmount *= ownerAttack.Value / 100f;

            float critRoll = Random.Range(0f, 100f);
            if (critRoll <= ownerCriticalChance.Value)
                adjustedAmount *= 1 + ownerCriticalDamage.Value / 100f;
        
        }

        // Target Adjustment

        CharacterResources targetResources = target.CharacterResources;
        CharacterStats targetStats = target.CharacterStats;

        Stat targetArmor = targetStats.GetStat(StatType.Armor, this);
        Stat targetShield = targetStats.GetStat(StatType.Shield, this);
        Stat targetDefense = targetStats.GetStat(StatType.Defense, this);

        adjustedAmount -= targetDefense.Value;
        adjustedAmount = Mathf.Max(1f, adjustedAmount);

        float armorMultiplier = 100 / (targetArmor.Value + 100);
        float shieldMultiplier = 100 / (targetShield.Value + 100);

        // Spawning damage particles and triggering animation

        Color color;

        switch (damageType)
        {
            case DamageType.Physical:
                adjustedAmount *= armorMultiplier;
                color = Color.red; 
                break;
            case DamageType.Magical:
                adjustedAmount *= shieldMultiplier;
                color = Color.blue;
                break;
            case DamageType.True:
                color = Color.white;
                break;
            default:
                color = Color.black;
                break;
        }

        if (adjustedAmount <= 0f)
            return;
 
        if (targetResources.ChangeResourceValue(
                ResourceType.Health,
                -1f * adjustedAmount * context.Magnitude,
                out float changed,
                resetRegenerationIfChanged)
        )
        {
            if (damageNumberPrefab != null)
            {
                GameObject number = Object.Instantiate(
                    damageNumberPrefab,
                    target.transform.position,
                    target.transform.rotation
                );
                number.GetComponentInChildren<DamageNumberUI>().Initialize(Mathf.Abs(changed), color);
            }

            if (damageParticlesPrefab != null)
                Object.Instantiate(
                    damageParticlesPrefab,
                    target.transform.position,
                    target.transform.rotation
                );

            target.CharacterAnimation.HandleHit();
        }
    }
}