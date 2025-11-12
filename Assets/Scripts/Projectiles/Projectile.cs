using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Collider), typeof(NonActorController))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private List<ProjectileBehaviorDefinition> behaviorDefinitions;
    private List<ProjectileBehavior> behaviors = new();
    public GameObject SourceActor { get; private set; }
    public GameObject TargetActor => controller.HomingTarget;

    [SerializeField] private bool collidesWithSource;
    [SerializeField] private int piercingAmount;
    private int _pierced;
    private bool ZeroPierce => _pierced <= 0;

    [SerializeField] private float duration;
    private float _timer;
    private bool ZeroTime => _timer <= 0;

    private NonActorController controller;

    public void SetSource(GameObject source)
    {
        if (source.layer == LayerMask.NameToLayer("Actors"))
            SourceActor = source;
    }

    void Awake()
    {
        controller = GetComponent<NonActorController>();
        GetComponent<Collider>().isTrigger = true;

        behaviors = behaviorDefinitions
            .Select(b => b.CreateRuntimeBehavior(this))
            .ToList();

        _timer = duration;
        _pierced = piercingAmount;
    }

    void Update()
    {
        float dt = Time.deltaTime;
        _timer -= dt;
        behaviors.ForEach(b => b.OnTick(dt));
    }

    // Only collides with actors and terrain (layer collision matrix)
    void OnTriggerEnter(Collider other)
    {
        GameObject target = other.transform.root.gameObject;
        
        if (target == null) return;
        if (target == SourceActor && !collidesWithSource) return;

        _pierced--;
        behaviors.ForEach(b => b.OnCollide(target));
        TryExpire();
    }

    // Expiring occurs when zero time or zero pierces left.
    bool TryExpire()
    {
        if (!ZeroPierce && !ZeroTime) return false;

        behaviors.ForEach(b => b.OnExpire());
        Destroy(gameObject);
        return true;
    }
}
