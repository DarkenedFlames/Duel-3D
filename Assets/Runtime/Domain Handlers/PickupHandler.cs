using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickupHandler : MonoBehaviour
{
    [Header("Pickup Definition")]
    public PickupDefinition pickupDefinition;
    Pickup pickup;

    /// <summary>
    /// Sets defaults and creates and stores logical Weapon.<br/>
    /// <see cref="SpawnerController.SpawnPickup"/> ensures <see cref="pickupDefinition"/> is not null before this.
    /// </summary>
    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;

        pickup = new Pickup(pickupDefinition, this);
    }
    void Update()
    {
        float dt = Time.deltaTime;
        pickup.GetAllGates().ForEach(g => g.Tick(dt));
        pickup.PerformHook(pickup.updateGate, b => b.OnUpdate(dt), nameof(Update));
    }
    void OnTriggerEnter(Collider other)
    {
        // Should be replaced with proper query, or we need a few tags like Player/AI/etc
        // Collide only with "Player" tags
        if (!other.CompareTag("Player")) return;
        pickup.PerformHook(pickup.triggerGate, b => b.OnTrigger(other), nameof(OnTriggerEnter));
    }

    /// <summary>
    /// Stores source.<br/>
    /// <see cref="SpawnerController.SpawnPickup"/>  ensures this is called after <see cref="Awake"/>. 
    /// </summary> 
    public void Spawn() => pickup.PerformHook(pickup.spawnGate, b => b.OnSpawn(), nameof(Spawn));
    public void Expire() 
    {
        pickup.PerformHook(pickup.expireGate, b => b.OnExpire(), nameof(Expire));
        Destroy(gameObject);
    }
}
