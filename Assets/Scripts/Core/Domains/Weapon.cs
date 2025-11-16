using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Weapon : MonoBehaviour, IHasSourceActor
{
    [Header("Weapon Stats")]
    public float staminaCost = 10f;
    [SerializeField] private float cooldownTime = 5f;
    [SerializeField] private float swingDuration = 1f;
    [SerializeField] private int piercesPerSwing = 1;
    public string animationTrigger = "AttackTrigger";
    public List<WeaponInstructionBinding> bindings;

    public GameObject SourceActor { get; set; }

    private Collider col;
    public event Action OnSwing;

    private float _cooldownTimer;
    private float _pierces;

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

        _cooldownTimer = cooldownTime;

        col.enabled = true;
        _pierces = piercesPerSwing;
        // PerformInstruction(WeaponHook.OnSwing, ??) // Don't know how to execute instruction without target

        StartCoroutine(SwingRoutine());
        return true;
    }

    private IEnumerator SwingRoutine()
    {
        yield return new WaitForSeconds(swingDuration);
        col.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.layer != LayerMask.NameToLayer("Actors")) return;

        PerformInstruction(WeaponHook.OnCollide, target);

        _pierces--;
        if (_pierces <= 0) 
            col.enabled = false;
    }

    void PerformInstruction(WeaponHook hook, GameObject target)
    {
        WeaponContext weaponContext = new(this, target);

        foreach (WeaponInstructionBinding binding in bindings)
            if (binding.Hook.Equals(hook))
                binding.Execute(weaponContext);
    }
}
