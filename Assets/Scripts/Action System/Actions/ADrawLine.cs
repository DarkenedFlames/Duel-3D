using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ADrawLine : IGameAction
{
    public enum TransformPoint
    {
        Owner,
        Source,
        Target
    }

    [Header("Conditions")]
    [SerializeReference]
    public List<IActionCondition> Conditions;

    [Header("Action Configuration")]
    [Tooltip("The line prefab to spawn."), SerializeField] 
    GameObject prefab;

    [Tooltip("Transform reference for the starting point of the line."), SerializeField]
    TransformPoint start = TransformPoint.Source;

    [Tooltip("Transform reference for the ending point of the line."), SerializeField]
    TransformPoint end = TransformPoint.Source;

    public void Execute(ActionContext context)
    {
        static Transform GetTransformPoint(ActionContext ctx, TransformPoint point)
        {
            return point switch
            {
                TransformPoint.Owner  => ctx.Source.Owner.transform,
                TransformPoint.Target => ctx.Target.transform,
                TransformPoint.Source => ctx.Source.Transform,
                _ => null
            };
        }

        Transform startTransform = GetTransformPoint(context, start);
        Transform endTransform = GetTransformPoint(context, end);

        if (prefab == null || startTransform == null || endTransform == null)
        {
            Debug.LogError("ADrawLine: Missing required references.");
            return;
        }
        
        if (Conditions?.Any(c => !c.IsSatisfied(context)) == true)
            return;

        GameObject lineObject = Object.Instantiate(prefab, startTransform.position, Quaternion.identity);
        if (!lineObject.TryGetComponent(out LineConnector lineConnector))
        {
            LogFormatter.LogMissingComponent(nameof(LineConnector), nameof(ADrawLine), lineObject);
            return;
        }

        lineConnector.AddTarget(startTransform);
        lineConnector.AddTarget(endTransform);
    }
}