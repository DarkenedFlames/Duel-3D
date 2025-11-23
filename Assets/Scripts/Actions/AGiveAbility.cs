using UnityEngine;

[System.Serializable]
public class AGiveAbility : IGameAction
{
    [Header("Ability Configuration")]
    [Tooltip("Ability to give."), SerializeField]
    AbilityDefinition abilityDefinition;

    public void Execute(GameObject source, GameObject target)
    {
        if (target == null) return;

        if (target.TryGetComponent(out CharacterAbilities abilityHandler))
            abilityHandler.LearnAbility(abilityDefinition);
    }
}