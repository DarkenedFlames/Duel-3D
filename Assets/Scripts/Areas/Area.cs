using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class Area : MonoBehaviour
{
    public List<AreaBehaviorDefinition> behaviorDefinitions;
    public float radius = 5f;
    public float duration = 5f;
    public GameObject sourceActor;

    // Live list of actors currently inside the area (excluding source if needed)
    private readonly HashSet<GameObject> currentTargets = new();
    public IReadOnlyCollection<GameObject> CurrentTargets => currentTargets;

    private readonly List<AreaBehavior> behaviors = new();
    private float lifetime;
    private bool expired;

    private void Awake()
    {
        var col = GetComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = radius;

        foreach (var def in behaviorDefinitions)
        {
            if (def != null)
                behaviors.Add(def.CreateRuntimeBehavior(this));
            else
                Debug.LogWarning($"{name} has a null AreaBehaviorDefinition slot.");
        }

        lifetime = duration;
    }

    public void SetSource(GameObject source) => sourceActor = source;

    private void Start() => behaviors.ForEach(b => b.OnStart());

    private void Update()
    {
        float dt = Time.deltaTime;

        lifetime -= dt;
        behaviors.ForEach(b => b.OnTick(dt));

        if (lifetime <= 0f && !expired) Expire();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Make sure to check if its an actor
        if (!currentTargets.Contains(other.gameObject))
            currentTargets.Add(other.gameObject);

        behaviors.ForEach(b => b.OnTargetEnter(other));
    }

    private void OnTriggerExit(Collider other)
    {
        currentTargets.Remove(other.gameObject);
        behaviors.ForEach(b => b.OnTargetExit(other));
    }

    private void Expire()
    {
        expired = true;
        behaviors.ForEach(b => b.OnExpire());
        Destroy(gameObject);
    }
}
