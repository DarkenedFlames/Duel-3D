
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(SpawnContext))]
public class Weapon : MonoBehaviour
{
    public WeaponDefinition Definition;
    public FloatCounter seconds;
    SpawnContext spawnContext;
    Collider col;

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

        Definition.OnHitActions.ForEach(a => a.Execute(gameObject, target));
    }

    bool FilterTarget(Collider other, out GameObject target)
    {
        target = null;
        GameObject potentialTarget = other.gameObject;
        if (spawnContext.Owner == potentialTarget)
            return false;
        if ((Definition.layerMask.value & (1 << potentialTarget.layer)) == 0)
            return false;

        return potentialTarget != null;
    }

    public bool TryUse()
    {   
        GameObject owner = spawnContext.Owner;
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
        col.enabled = true;
        yield return new WaitForSeconds(Definition.UseTime);
        col.enabled = false;
    }
}