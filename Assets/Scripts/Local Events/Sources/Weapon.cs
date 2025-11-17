using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class Weapon : LocalEventSource, IHasSourceActor
{
    [Header("Weapon Stats")]
    public float staminaCost = 10f;
    [SerializeField] float cooldownTime = 5f;
    [SerializeField] float swingDuration = 1f;
    [SerializeField] int piercesPerSwing = 1;
    public string animationTrigger = "AttackTrigger";

    public GameObject SourceActor { get; set; }

    private Collider col;

    private float _cooldownTimer;
    private float _pierces;

    readonly HashSet<GameObject> CurrentTargets = new();

    void Awake()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;
        col.enabled = false;
    }

    public void SetSource(GameObject source)
    {
        SourceActor = source;
        Physics.IgnoreCollision(source.GetComponent<Collider>(), col, true);
    }

    void Update()
    {
        _cooldownTimer -= Time.deltaTime;
    }

    public bool TrySwing()
    {
        if (_cooldownTimer > 0f) return false;

        CurrentTargets.Clear();
        _cooldownTimer = cooldownTime;
        _pierces = piercesPerSwing;
        col.enabled = true;

        Fire(Event.OnSwing, new NullContext(){});
        StartCoroutine(SwingRoutine());
        col.enabled = false;
        return true;
    }

    private IEnumerator SwingRoutine()
    {
        yield return new WaitForSeconds(swingDuration);
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject targetObj = other.gameObject;
        if (targetObj.layer != LayerMask.NameToLayer("Actors")) return;
        if (CurrentTargets.Contains(targetObj)) return;

        CurrentTargets.Add(targetObj);
        Fire(Event.OnCollide, new PositionContext() {target = targetObj, localTransform = transform});

        _pierces--;
        if (_pierces <= 0)
            col.enabled = false;
    }
}
