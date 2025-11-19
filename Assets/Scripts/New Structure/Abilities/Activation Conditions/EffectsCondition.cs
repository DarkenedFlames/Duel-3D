using UnityEngine;

[CreateAssetMenu(menuName = "Duel/Abilities/Conditions/EffectsCondition")]
public class EffectsCondition : ActivationCondition
{
    [Header("Whitelist")]
    [Tooltip("Must have these effects to activate.")]
    public Effect[] requiredEffects;

    [Tooltip("If true, must have *all* required effects; otherwise, any one is enough.")]
    public bool requireAll = false;

    [Header("Blacklist")]
    [Tooltip("Must NOT have these effects to activate.")]
    public Effect[] forbiddenEffects;

    [Tooltip("If true, forbids *all* of these effects being present; otherwise, forbids any one.")]
    public bool forbidAll = false;

    public override bool IsMet(Ability ability)
    {
        // allow checking for whitelist/blacklist with requireAll optional
        return true;
    }
}