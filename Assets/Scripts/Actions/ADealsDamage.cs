using System.Collections.Generic;
using UnityEngine;

public enum DamageType { Physical, Magical, True }

[System.Serializable]
public class ADealsDamage : IGameAction
{
    [Header("Conditions")]
    [SerializeReference]
    public List<IActionCondition> Conditions;

    [Header("Action Configuration")]
    [Tooltip("Who to damage: Owner (caster/summoner) or Target (hit character)."), SerializeField]
    ActionTargetMode targetMode = ActionTargetMode.Target;

    [Tooltip("Whether to reset regeneration if damage is dealt."), SerializeField]
    bool resetRegenerationIfChanged = true;

    [Tooltip("The amount of damage dealt."), SerializeField, Min(0)]
    float amount = 1f;

    [Tooltip("The type of damage dealt."), SerializeField]
    DamageType damageType = DamageType.Physical;

    [SerializeField] GameObject damageNumberPrefab;

    [SerializeField] GameObject normalHitParticlesPrefab;
    [SerializeField] GameObject criticalHitParticlesPrefab;

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
        bool didCrit = false;
        Character owner = context.Source.Owner;
        if (owner != null)
        {
            CharacterStats ownerStats = owner.CharacterStats;
            Stat ownerAttack = ownerStats.GetStat(StatType.Attack);
            Stat ownerCriticalChance = ownerStats.GetStat(StatType.CriticalChance);
            Stat ownerCriticalDamage = ownerStats.GetStat(StatType.CriticalDamage);

            adjustedAmount *= ownerAttack.Value / 100f;

            float critRoll = Random.Range(0f, 100f);
            if (critRoll <= ownerCriticalChance.Value)
            {
                adjustedAmount *= 1 + ownerCriticalDamage.Value / 100f;
                didCrit = true;
            }
        }

        // Target Adjustment

        CharacterResources targetResources = target.CharacterResources;
        CharacterStats targetStats = target.CharacterStats;

        Stat targetArmor = targetStats.GetStat(StatType.Armor);
        Stat targetShield = targetStats.GetStat(StatType.Shield);
        Stat targetDefense = targetStats.GetStat(StatType.Defense);

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

            GameObject damageParticlesPrefab = didCrit ? criticalHitParticlesPrefab : normalHitParticlesPrefab;

            if (damageParticlesPrefab != null)
                Object.Instantiate(
                    damageParticlesPrefab,
                    target.transform.position + Vector3.up,
                    target.transform.rotation
                );

            target.CharacterAnimation.HandleHit();
        }
    }
}