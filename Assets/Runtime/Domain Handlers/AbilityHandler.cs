using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationHandler), typeof(StatsHandler))]
public class AbilityHandler : MonoBehaviour
{
    [Header("Initial Ability Definitions")]
    public AbilityDefinition initialPrimary;
    public AbilityDefinition initialSecondary;
    public AbilityDefinition initialUtility;
    public AbilityDefinition initialSpecial;

    // Runtime instances
    Ability primary;
    Ability secondary;
    Ability utility;
    Ability special;

    // === Convenience Utilities ===
    Ability GetAbility(AbilityType type) => type switch
    {
        AbilityType.Primary => primary,
        AbilityType.Secondary => secondary,
        AbilityType.Utility => utility,
        AbilityType.Special => special,
        _ => null
    };

    List<Ability> GetAllAbilities() => new() { primary, secondary, utility, special };

    // === Hooks ===
    void Awake()
    {
        // Initialize default abilities if assigned, eventually guarantee them as non-null
        List<AbilityDefinition> defs = new() { initialPrimary, initialSecondary, initialUtility, initialSpecial };

        defs.ForEach(def =>
            {
                if (def != null) Spawn(def);
                else Debug.LogWarning($"{gameObject.name} is missing an initial ability definition.");
            }
        );
    }

    void Update()
    {
        float dt = Time.deltaTime;
        GetAllAbilities().ForEach(a => a.GetAllGates().ForEach(g => g.Tick(dt)));
        GetAllAbilities().ForEach(a => a?.PerformHook(a.updateGate, b => b.OnUpdate(dt), nameof(Update)));
        // Can remove ? once we ensure abilities will always be present
    }

    // Eventually will be called by a pickup behavior
    public void Spawn(AbilityDefinition definition)
    {
        // Not spawned by SpawnController, but similarly validates.
        // See if there's room for central validation.
        if (definition == null)
        {
            Debug.LogWarning("Tried to equip null ability definition!");
            return;
        }

        Ability ability = new(definition, this);

        switch (definition.abilityType)
        {
            case AbilityType.Primary: primary = ability; break;
            case AbilityType.Secondary: secondary = ability; break;
            case AbilityType.Utility: utility = ability; break;
            case AbilityType.Special: special = ability; break;
        }
    }

    /// <summary>
    /// Called by the following methods on <see cref="gameObject"/>.<br/>
    /// <see cref="InputHandler.OnPrimaryCastStarted"/><br/>
    /// <see cref="InputHandler.OnSecondaryCastStarted"/><br/>
    /// <see cref="InputHandler.OnUtilityCastStarted"/><br/>
    /// <see cref="InputHandler.OnSpecialCastStarted"/><br/>
    /// </summary>
    public void Cast(AbilityType type)
    {
        Ability ability = GetAbility(type);
        ability.PerformHook(ability.castGate, b => b.OnCast(), nameof(Cast));
    }
}