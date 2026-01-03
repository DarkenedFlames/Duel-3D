using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ADestroyThis : IGameAction
{

    [Header("Conditions"), SerializeReference]
    public List<IActionCondition> Conditions;

    public void Execute(ActionContext context)
    {
        if (Conditions?.Any(c => !c.IsSatisfied(context)) == true) return;

        if (context.Source.GameObject != null)
            Object.Destroy(context.Source.GameObject);
    }
}