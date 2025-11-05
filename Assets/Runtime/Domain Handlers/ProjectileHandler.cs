using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ProjectileHandler : MonoBehaviour
{
    public ProjectileDefinition projectileDefinition;
    [System.NonSerialized] public Projectile projectile;
    [System.NonSerialized] public GameObject source;

    /// <summary>
    /// Sets defaults and creates and stores logical Weapon.<br/>
    /// <see cref="SpawnerController.SpawnProjectile"/> ensures <see cref="projectileDefinition"/> is not null before this.
    /// </summary>
    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = false;

        projectile = new(projectileDefinition, this);
    }
    void Update()
    {
        float dt = Time.deltaTime;
        projectile.GetAllGates().ForEach(g => g.Tick(dt));
        projectile.PerformHook(projectile.updateGate, b => b.OnUpdate(dt), nameof(Update));
    }
    void OnTriggerEnter(Collider other)
    {
        if (source != null && other.transform.IsChildOf(source.transform) && !projectile.Definition.collidesWithSource)
            return;

        if (other.CompareTag("Ground") && !projectile.Definition.collidesWithTerrain)
            return;

        projectile.PerformHook(projectile.triggerGate, b => b.OnTrigger(other), nameof(OnTriggerEnter));
        Expire();
    }
    
    /// <summary>
    /// Stores source.<br/>
    /// <see cref="SpawnerController.SpawnProjectile"/>  ensures this is called after <see cref="Awake"/>. 
    /// </summary> 
    public void Spawn(GameObject sourceObject)
    {
        // Make SpawnController validate source
        source = sourceObject;
        projectile.PerformHook(projectile.spawnGate, b => b.OnSpawn(), nameof(Spawn));
    }
    public void Expire()
    {
        projectile.PerformHook(projectile.expireGate, b => b.OnExpire(), nameof(Expire));
        Destroy(gameObject);
    }
}
