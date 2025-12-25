using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class LightOrbit : MonoBehaviour
{
    [Header("Lights")]
    [SerializeField] List<LightOrbitEntry> lights = new();

    [Header("Orbit Settings")]
    [SerializeField] float period = 600f;
    [SerializeField] Vector3 worldCenter = new(500f, 0f, 500f);
    [SerializeField] float radius = 1000f;
    [SerializeField] Gradient skyboxGradient = new();
    
    FloatCounter seconds;
    void Awake() => seconds = new(0, 0, period, resetToMax: false);
    void Update()
    {
        seconds.Increase(Time.deltaTime);
        if (seconds.Exceeded)
            seconds.Reset();

        RenderSettings.skybox.color = skyboxGradient.Evaluate(seconds.Progress);

        foreach (LightOrbitEntry light in lights)
        {
            float angle = seconds.Progress * Mathf.PI * 2f + Mathf.Deg2Rad * light.Offset;
            Vector3 direction = new(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            Vector3 position = worldCenter + radius * direction;
            light.transform.SetPositionAndRotation(position, Quaternion.LookRotation(worldCenter - position));

            Light lightComponent = light.Light;
            lightComponent.intensity = light.IntensityCurve.Evaluate(seconds.Progress);
            lightComponent.color = light.ColorGradient.Evaluate(seconds.Progress);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(worldCenter, 10f);
    }
}