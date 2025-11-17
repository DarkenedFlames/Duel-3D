using UnityEngine;

[System.Serializable]
public class LGiveAbility : EventReaction
{
    [Header("GiveAbility Configuration")]
    [Tooltip("Ability to give."), SerializeField]
    AbilityDefinition abilityDefinition;

    public override void OnEvent(EventContext context)
    {
        if (context is TargetContext cxt)
            if (cxt.target.TryGetComponent(out AbilityHandler abilityHandler))
                abilityHandler.LearnAbility(abilityDefinition);
    }
}