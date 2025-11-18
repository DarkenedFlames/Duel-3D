using UnityEngine;

[System.Serializable]
public class LGiveAbility : EventReaction
{
    [Header("GiveAbility Configuration")]
    [Tooltip("Ability to give."), SerializeField]
    AbilityDefinition abilityDefinition;

    public override void OnEvent(EventContext context)
    {
        if (context.defender == null) return;

        if (context.defender.TryGetComponent(out AbilityHandler abilityHandler))
            abilityHandler.LearnAbility(abilityDefinition);
    }
}