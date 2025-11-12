using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public static SpawnerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Spawns a prefab that must include a component of type T.
    /// Returns the component if successful, or null if the prefab is invalid.
    /// </summary>
    public T SpawnValidated<T>(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : Component
    {
        if (prefab == null)
        {
            Debug.LogError($"Tried to spawn a null prefab of type {typeof(T).Name}.");
            return null;
        }

        GameObject instance = Instantiate(prefab, position, rotation, parent);

        if (!instance.TryGetComponent(out T component))
        {
            Debug.LogError($"Spawned prefab {prefab.name} is missing required component {typeof(T).Name}. Destroying instance.");
            Destroy(instance);
            return null;
        }

        // --- Definition validation section ---
        bool invalid = false;
        switch (component)
        {
            case WeaponHandler weapon when weapon.weaponDefinition == null:
                Debug.LogError($"Spawned weapon {prefab.name} is missing its WeaponDefinition. Destroying instance.");
                invalid = true;
                break;
        }

        if (invalid)
        {
            Destroy(instance);
            return null;
        }

        return component;
    }

    // --- Specialized spawn methods ---
    // Make these validate source/wielder as well.
    public WeaponHandler SpawnWeapon(GameObject weaponPrefab, GameObject wielder)
    {
        WeaponHandler handler = SpawnValidated<WeaponHandler>(weaponPrefab, wielder.transform.position, Quaternion.identity);
        if (handler != null) handler.Spawn(wielder);
        return handler;
    }

    public Projectile SpawnProjectile(GameObject projectilePrefab, Vector3 position, Quaternion rotation, GameObject source = null)
    {
        Projectile projectile = SpawnValidated<Projectile>(projectilePrefab, position, rotation);
        projectile.SetSource(source);
        return projectile;
    }

    public Pickup SpawnPickup(GameObject pickupPrefab, Vector3 position, Quaternion rotation)
    {
        return SpawnValidated<Pickup>(pickupPrefab, position, rotation);
    }

    public Area SpawnArea(GameObject areaPrefab, Vector3 position, Quaternion rotation, GameObject source = null)
    {
        Area area = SpawnValidated<Area>(areaPrefab, position, rotation);
        area.SetSource(source);
        return area;
    }

}
