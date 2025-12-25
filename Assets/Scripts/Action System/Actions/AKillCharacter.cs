using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AKillCharacter : IGameAction
{
    [Header("Conditions")]
    [SerializeReference]
    public List<IActionCondition> Conditions;

    [Header("Action Configuration")]
    [Tooltip("Who to damage: Owner (caster/summoner) or Target (hit character)."), SerializeField]
    ActionTargetMode targetMode = ActionTargetMode.Target;

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

        target.CharacterResources.Kill();
    }
}