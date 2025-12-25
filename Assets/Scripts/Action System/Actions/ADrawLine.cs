using UnityEngine;
using System.Collections.Generic;

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
        if (prefab == null)
        {
            LogFormatter.LogNullField(nameof(prefab), nameof(ASpawnObject), context.Source.GameObject);
            return;
        }

        Transform startTransform = start switch
        {
            TransformPoint.Owner  => context.Source.Owner.transform,
            TransformPoint.Target => context.Target.transform,
            TransformPoint.Source => context.Source.Transform,
            _ => null
        };

        Transform endTransform = end switch
        {
            TransformPoint.Owner  => context.Source.Owner.transform,
            TransformPoint.Target => context.Target.transform,
            TransformPoint.Source => context.Source.Transform,
            _ => null
        }; 

        if (startTransform == null)
        {
            LogFormatter.LogNullArgument(nameof(startTransform), nameof(Execute), nameof(ADrawLine), context.Source.GameObject);
            return;
        }

        if (endTransform == null)
        {
            LogFormatter.LogNullArgument(nameof(endTransform), nameof(Execute), nameof(ADrawLine), context.Source.GameObject);
            return;
        }

        if (Conditions != null)
        {
            foreach (IActionCondition condition in Conditions)
                if (!condition.IsSatisfied(context))
                    return;
        }


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