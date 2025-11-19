using UnityEditor.PackageManager;
using UnityEngine;

[CreateAssetMenu(menuName = "Duel/Abilities/Conditions/StatusCondition")]
public class StatusCondition : ActivationCondition
{
    [Header("Whitelist")]
    [Tooltip("Must have these statuses to activate.")]
    public Status requiredStatus = Status.None;

    [Tooltip("If true, must have *all* required statuses; otherwise, any one is enough.")]
    public bool requireAll = false;

    [Header("Blacklist")]
    [Tooltip("Must NOT have these statuses to activate.")]
    public Status forbiddenStatus = Status.None;

    [Tooltip("If true, forbids *all* of these statuses being present; otherwise, forbids any one.")]
    public bool forbidAll = false;

    public override bool IsMet(Ability ability)
    {
        if (!ability.Handler.TryGetComponent(out StatusHandler statusHandler))
            return false;
        
        // Whitelist logic
        bool whitelistOk = requiredStatus == Status.None ||
            (requireAll
                ? statusHandler.MatchStatus(requiredStatus)   // all required bits are present
                : statusHandler.HasStatus(requiredStatus));   // any one required bit is present

        // Blacklist logic
        bool blacklistOk = forbiddenStatus == Status.None ||
            (forbidAll
                ? !statusHandler.MatchStatus(forbiddenStatus)  // none of the forbidden bits are present
                : !statusHandler.HasStatus(forbiddenStatus));  // not *any* forbidden bit is present

        return whitelistOk && blacklistOk;
    }
}