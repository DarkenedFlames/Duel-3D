using UnityEngine;

public class RGiveAbility : Reaction
{
    [Header("GiveAbility Configuration")]
    [Tooltip("Ability to give."), SerializeField]
    AbilityDefinition abilityDefinition;

    public void OnEvent(GameObject target)
    {
        if (target == null) return;

        if (target.TryGetComponent(out AbilityHandler abilityHandler))
            abilityHandler.LearnAbility(abilityDefinition);
    }
}