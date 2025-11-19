using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(CollisionCollector))]
[RequireComponent(typeof(LocalEventSource))]
public class WeaponSwing : MonoBehaviour
{
    [Header("Swing Settings")]
    public float swingStaminaCost = 10f;
    [SerializeField] float cooldownTime = 5f;
    [SerializeField] float swingDuration = 1f;
    [SerializeField] int hitsPerSwing = 1;
    public string animationTrigger = "AttackTrigger";

    Collider col;
    LocalEventSource eventSource;

    Counter seconds;

    void Awake()
    {
        col = GetComponent<Collider>();
        col.enabled = false;

        eventSource = GetComponent<LocalEventSource>();
        seconds = new(cooldownTime);
    }

    void Update()
    {
        seconds.Decrease(Time.deltaTime);
    }

    public bool TrySwing()
    {
        if (!seconds.Expired) 
            return false;

        seconds.Reset();

        col.enabled = true;

        eventSource.Fire(Event.OnSwing, new EventContext{});

        StartCoroutine(SwingRoutine());
        return true;
    }

    private IEnumerator SwingRoutine()
    {
        yield return new WaitForSeconds(swingDuration);

        col.enabled = false;
    }

}
