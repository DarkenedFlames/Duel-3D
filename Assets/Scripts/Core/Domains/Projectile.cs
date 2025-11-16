using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider), typeof(NonActorController))]
public class Projectile : MonoBehaviour, IHasSourceActor
{
    [SerializeField] private bool collidesWithSource;
    [SerializeField] private int piercingAmount;
    [SerializeField] private float duration;
    public List<ProjectileInstructionBinding> bindings = new();

    private int _pierced;
    private bool ZeroPierce => _pierced <= 0;

    private float _timer;
    private bool ZeroTime => _timer <= 0;

    private NonActorController controller;

    public GameObject SourceActor { get; set; }
    public GameObject TargetActor => controller.HomingTarget;

    public void SetSource(GameObject source)
    {
        if (source.layer == LayerMask.NameToLayer("Actors"))
            SourceActor = source;
    }

    void Awake()
    {
        controller = GetComponent<NonActorController>();
        GetComponent<Collider>().isTrigger = true;

        _timer = duration;
        _pierced = piercingAmount;
    }

    void Update()
    {
        float dt = Time.deltaTime;
        _timer -= dt;
        // PerformInstruction(); // no target, not sure how to fire
        TryExpire();
    }

    // Only collides with Actors and Terrain
    void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        
        if (target.layer != LayerMask.NameToLayer("Actors")) return;
        if (target == SourceActor && !collidesWithSource) return;

        _pierced--;
        PerformInstruction(ProjectileHook.OnCollide, target);
        TryExpire();
    }

    // Expiring occurs when zero time or zero pierces left.
    bool TryExpire()
    {
        if (!ZeroPierce && !ZeroTime) return false;

        // PerformInstruction(ProjectileHook.OnExpire, ) // no target, not sure how to fire
        Destroy(gameObject);
        return true;
    }

    void PerformInstruction(ProjectileHook hook, GameObject target)
    {
        ProjectileContext projectileContext = new(this, target);

        foreach (ProjectileInstructionBinding binding in bindings)
            if (binding.Hook.Equals(hook))
                binding.Execute(projectileContext);
    }
}
