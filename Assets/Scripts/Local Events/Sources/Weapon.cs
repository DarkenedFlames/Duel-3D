using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(ActorTargeting))]
public class Weapon : LocalEventSource, IHasSourceActor
{
    [Header("Weapon Stats")]
    public float staminaCost = 10f;
    [SerializeField] float cooldownTime = 5f;
    [SerializeField] float swingDuration = 1f;
    [SerializeField] int hitsPerSwing = 1;
    public string animationTrigger = "AttackTrigger";

    public GameObject SourceActor { get; set; }

    private Collider col;
    private ActorTargeting targeting;

    private float _cooldownTimer;
    private int _hits;

    void Awake()
    {
        col = GetComponent<Collider>();
        col.enabled = false;

        targeting = GetComponent<ActorTargeting>();
    }

    void OnEnable()
    {
        targeting.OnActorEnter += HandleActorEnter;
    }

    void OnDisable()
    {
        targeting.OnActorEnter -= HandleActorEnter;
    }

    public void SetSource(GameObject source)
    {
        SourceActor = source;

        if (source.TryGetComponent(out Collider srcCol))
            Physics.IgnoreCollision(srcCol, col, true);
    }

    void Update()
    {
        _cooldownTimer -= Time.deltaTime;
    }

    public bool TrySwing()
    {
        if (_cooldownTimer > 0f) 
            return false;

        _cooldownTimer = cooldownTime;
        _hits = 0;

        col.enabled = true;

        Fire(Event.OnSwing, new NullContext());

        StartCoroutine(SwingRoutine());
        return true;
    }

    private IEnumerator SwingRoutine()
    {
        yield return new WaitForSeconds(swingDuration);

        // End swing
        col.enabled = false;
    }

    private void HandleActorEnter(GameObject actor)
    {
        // Ignore source actor
        if (actor == SourceActor)
            return;

        Fire(Event.OnCollide, new PositionContext()
        {
            target = actor,
            localTransform = transform
        });

        _hits++;

        if (_hits >= hitsPerSwing)
            col.enabled = false;
    }
}
