using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class MLinearScale : MonoBehaviour
{
    [Header("Linear Scale Sequence")]
    [Tooltip("Series of duration + target scale for sequential resizing."), SerializeField]
    List<FloatVector3Pair> ScaleSequence = new();

    Vector3 currentStartingScale;

    IntegerCounter steps;
    FloatCounter seconds;

    FloatVector3Pair CurrentPair => ScaleSequence[steps.Value];
    float CurrentFraction => Mathf.Clamp01(seconds.Value / CurrentPair.Float);

    void Start()
    {
        currentStartingScale = transform.localScale;

        if (ScaleSequence.Count == 0)
        {
            LogFormatter.LogNullCollectionField(nameof(ScaleSequence), nameof(Start), nameof(MLinearScale), gameObject);
            return;
        }

        steps = new(0, 0, ScaleSequence.Count - 1, resetToMax: false);
        seconds = new(0, 0, CurrentPair.Float, resetToMax: false);
    }

    void Update()
    {
        seconds.Increase(Time.deltaTime);
        transform.localScale = Vector3.Lerp(currentStartingScale, CurrentPair.Vector, CurrentFraction);

        if (CurrentFraction < 1f) return;

        steps.Increment();
        if (steps.Exceeded) steps.Reset();

        currentStartingScale = transform.localScale;
        seconds.SetMax(CurrentPair.Float);
        seconds.Reset();
    }
}
