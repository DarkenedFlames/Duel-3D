
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Weapon : MonoBehaviour, IRequiresSource
{
    public WeaponDefinition Definition;
    public GameObject Source { get; set; }

    public FloatCounter seconds;
    Collider col;

    void Awake()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
        seconds = new(Definition.CooldownTime, 0, Definition.CooldownTime, true, true);
    }

    void Update()
    {
        seconds.Decrease(Time.deltaTime);
    }

    public bool TryUse()
    {        
        CharacterStats sourceStats = Source.GetComponent<CharacterStats>();

        string expendedStatName = Definition.ExpendedStat.statName;
        float amountToExpend = Definition.StatCost;

        if (sourceStats.TryGetStat(expendedStatName, out ClampedStat stat) 
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