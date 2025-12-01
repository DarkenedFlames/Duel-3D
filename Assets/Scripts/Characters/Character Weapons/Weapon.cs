
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Weapon : MonoBehaviour, IActionSource
{
    public Character Owner { get; set; }
    public Transform Transform => transform;
    public GameObject GameObject => gameObject;

    public WeaponDefinition Definition;
    public FloatCounter seconds;

    Collider col;

    HashSet<GameObject> _hitThisSwing = new();

    void Awake()
    {
        Owner = GetComponentInParent<Character>();
        if (Owner == null)
            Debug.LogError($"{Owner.gameObject.name}'s {Definition.WeaponName} expected a {nameof(Character)} component but it was missing!");

        col = GetComponent<Collider>();
        col.enabled = false;
        seconds = new(Definition.CooldownTime, 0, Definition.CooldownTime, true, true);
    }

    void Update() => seconds.Decrease(Time.deltaTime);
    
    void OnTriggerEnter(Collider other)
    {
        if (!FilterTarget(other, out GameObject target)) return;

        _hitThisSwing.Add(target);
        Execute(Definition.OnHitActions, target);
    }

    bool FilterTarget(Collider other, out GameObject target)
    {
        target = null;
        GameObject potentialTarget = other.gameObject;

        if (_hitThisSwing.Contains(potentialTarget)) // Already hit this swing => filter out
            return false;
        if (potentialTarget.TryGetComponent(out Character character) && character == Owner) // Is the wielder => filter out
            return false;
        if ((Definition.layerMask.value & (1 << potentialTarget.layer)) == 0) // Not on a valid layer => filter out
            return false;
        
        target = potentialTarget;
        return target != null;
    }

    public bool TryUse()
    {   
        CharacterResources resources = Owner.CharacterResources;

        if (resources.TryGetResource(Definition.ExpendedResource, out CharacterResource resource) 
            && Definition.ResourceCost <= resource.Value
            && seconds.Expired)
        {
            resource.ChangeValue(-1f * Definition.ResourceCost, out float _);
            resource.RegenerationCounter.Reset();
            
            seconds.Reset();
            StartCoroutine(UseCoroutine());
            return true;
        }
        return false;
    }

    IEnumerator UseCoroutine()
    {
        _hitThisSwing.Clear();
        col.enabled = true;
        yield return new WaitForSeconds(Definition.UseTime);
        col.enabled = false;
    }

    void Execute(List<IGameAction> actions, GameObject target)
    {
        if (!target.TryGetComponent(out Character character)) return;
        
        ActionContext context = new(){ Source = this, Target = character };
        actions.ForEach(a => a.Execute(context));
    }
}