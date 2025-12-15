using UnityEngine;

public abstract class SpawnerBase<T, TSpawnPoint, TSpawnPointSet> : MonoBehaviour
    where T : Component
    where TSpawnPoint : SpawnPoint<T>
    where TSpawnPointSet : SpawnPointSet<T, TSpawnPoint>
{
    [Header("Spawner Settings")]
    [SerializeField, Tooltip("Time in seconds between spawn attempts")]
    protected float spawnInterval = 5f;

    [SerializeField, Tooltip("The prefab to spawn")]
    protected GameObject prefabToSpawn;

    [SerializeField, Tooltip("The runtime set of spawn points to use")]
    protected TSpawnPointSet spawnPointSet;

    protected FloatCounter timer;

    void Start()
    {
        if (prefabToSpawn == null)
        {
            Debug.LogError($"{name}: No prefab assigned to spawn!");
            enabled = false;
            return;
        }

        if (spawnPointSet == null)
        {
            Debug.LogError($"{name}: No spawn point set assigned!");
            enabled = false;
            return;
        }

        timer = new FloatCounter(0, 0, spawnInterval, resetToMax: false);
    }

    void Update()
    {
        timer.Increase(Time.deltaTime);
        
        if (timer.Exceeded)
        {
            spawnPointSet.SpawnObject(prefabToSpawn);
            timer.Reset();
        }
    }
}
