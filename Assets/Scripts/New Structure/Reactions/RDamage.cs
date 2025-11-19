using UnityEngine;

public class RDamage : Reaction
{
    [Tooltip("The amount of damage dealt."), SerializeField]
    int amount;

    void DealDamage(GameObject target)
    {
        if (target.TryGetComponent(out StatsHandler stats))
            stats.TakeDamage(amount);
    }

    void OnTriggerEnter(Collider other)
    {
        if (ShouldReact(Event.OnTriggerEnter))
            DealDamage(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (ShouldReact(Event.OnTriggerExit))
            DealDamage(other.gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        if (ShouldReact(Event.OnTriggerStay))
            DealDamage(other.gameObject);
    }
}