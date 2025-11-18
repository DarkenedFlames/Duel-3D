using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColliderPulser : MonoBehaviour
{
    [Tooltip("The period in seconds at which collisions are enabled for one frame."), SerializeField]
    float pulsePeriod = 1f;

    Counter seconds;

    void Awake()
    {
        seconds = new Counter(pulsePeriod);
    }

    void Update()
    {
        seconds.Decrease(Time.deltaTime);

        Collider col = GetComponent<Collider>();
        
        if (!seconds.Expired)
            col.enabled = false;
        else
        {
            col.enabled = true;
            seconds.Reset();
        }
    }
}