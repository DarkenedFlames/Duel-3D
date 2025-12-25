using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineConnector : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField, Min(0)] float duration = 0f;
    [SerializeField, Min(0)] float segmentDuration = 0f;
    [SerializeField] bool destroyOnTargetLoss = false;
    [SerializeField] bool destroyOnNoSegments = false;
    [SerializeField] Vector3 offset = new(0, 1, 0);

    
    Dictionary<FloatCounter, Transform> segmentTargets = new();

    FloatCounter seconds;

    void Awake()
    {
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = 0;
        
        if (!Mathf.Approximately(0, duration))
            seconds = new(duration, 0, duration);
    }

    public void AddTarget(Transform target)
    {
        float timerDuration = Mathf.Approximately(0, segmentDuration) 
            ? float.MaxValue 
            : segmentDuration;
        FloatCounter timer = new(timerDuration, 0, timerDuration);
        segmentTargets.Add(timer, target);
    }

    void Update()
    {
        seconds?.Decrease(Time.deltaTime);
        if (seconds != null && seconds.Expired)
        {
            Destroy(gameObject);
            return;
        }

        List<FloatCounter> expiredCounters = new();
        foreach (var kvp in segmentTargets)
        {
            FloatCounter timer = kvp.Key;
            timer?.Decrease(Time.deltaTime);
            if (timer != null && timer.Expired)
                expiredCounters.Add(timer);
        }
        foreach (var expired in expiredCounters)
            segmentTargets.Remove(expired);

        List<Vector3> targets = new();
        List<FloatCounter> nullTransforms = new();
        foreach (var kvp in segmentTargets)
        {
            Transform target = kvp.Value;
            if (target != null)
            {
                targets.Add(target.position + offset);
            }
            else if (destroyOnTargetLoss)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                nullTransforms.Add(kvp.Key);
            }
        }
        foreach (var nullKey in nullTransforms)
            segmentTargets.Remove(nullKey);

        if (destroyOnNoSegments && segmentTargets.Count == 0)
        {
            Destroy(gameObject);
            return;
        }

        lineRenderer.positionCount = targets.Count();
        lineRenderer.SetPositions(targets.ToArray());
    }
}
