
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(SpawnContext))]
public class Weapon : MonoBehaviour
{
    public WeaponDefinition Definition;
    public FloatCounter seconds;
    SpawnContext spawnContext;
    Collider col;

    HashSet<GameObject> _hitThisSwing = new();

    void Awake()
    {
        col = GetComponent<Collider>();
        spawnContext = GetComponent<SpawnContext>();
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
        if (potentialTarget.TryGetComponent(out Character character) && character == spawnContext.Owner) // Is the wielder => filter out
            return false;
        if ((Definition.layerMask.value & (1 << potentialTarget.layer)) == 0) // Not on a valid layer => filter out
            return false;
        
        target = potentialTarget;
        return target != null;
    }

    public bool TryUse()
    {   
        Character owner = spawnContext.Owner;
        CharacterStats ownerStats = owner.GetComponent<CharacterStats>();

        string expendedStatName = Definition.ExpendedStat.statName;
        float amountToExpend = Definition.StatCost;

        if (ownerStats.TryGetStat(expendedStatName, out ClampedStat stat) 
            && amountToExpend <= stat.Value
            && seconds.Expired)
        {
            stat.BaseValue -= amountToExpend;
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