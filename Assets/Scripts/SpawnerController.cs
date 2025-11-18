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
        return component;
    }

    // --- Specialized spawn methods ---
    // Make these validate source/wielder as well.
    public GameObject SpawnArea(GameObject areaPrefab, Vector3 position, Quaternion rotation, GameObject sourceActor)
    {
        GameObject Area = Instantiate(areaPrefab, position, rotation);
        if (!Area.TryGetComponent(out IHasSourceActor hasSource))
            Debug.LogError($"Area: {Area.name} instantiated with no IHasSourceActor");

        hasSource.SetSource(sourceActor);
        return Area;
    }
}
