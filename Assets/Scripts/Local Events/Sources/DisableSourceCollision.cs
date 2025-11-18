using UnityEngine;

[RequireComponent(typeof(RequiresSource))]
[RequireComponent(typeof(Collider))]
public class DisableSourceCollison : MonoBehaviour
{
    void Awake()
    {
        GameObject source = GetComponent<RequiresSource>().Source;
        Collider col = GetComponent<Collider>();
        
        if (source != null && source.TryGetComponent(out Collider c))
            Physics.IgnoreCollision(c, col, true);
    }
}