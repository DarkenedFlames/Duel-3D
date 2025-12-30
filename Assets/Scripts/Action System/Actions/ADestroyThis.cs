using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ADestroyThis : IGameAction
{

    [Header("Conditions")]
    [SerializeReference]
    public List<IActionCondition> Conditions;

    public void Execute(ActionContext context)
    {
        if (context.Source.GameObject == null)
        {
            LogFormatter.LogNullArgument(nameof(context.Source.GameObject), nameof(Execute), nameof(ADestroyThis), null);
            return;
        }

        if (Conditions != null)
        {
            foreach (IActionCondition condition in Conditions)
                if (!condition.IsSatisfied(context))
                    return;
        }

        Object.Destroy(context.Source.GameObject);
    }
}