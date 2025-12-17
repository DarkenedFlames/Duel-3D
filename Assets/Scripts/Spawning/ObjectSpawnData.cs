using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class ObjectSpawnData
{
    public enum Mode
    {
        SpecificPrefab,
        RandomPrefabFromSet
    }

    [Tooltip("The mode to use for spawning."), SerializeField]
    Mode mode = Mode.SpecificPrefab;

    [Tooltip("The prefab to spawn at a position in the list."), SerializeField]
    GameObject prefab;

    [Tooltip("The set of prefabs to choose from for spawning."), SerializeField]
    PrefabSet set;

    [Tooltip("The timer interval to use for this set of positions."), SerializeField]
    float interval = 5f;

    [Tooltip("The world-space positions to spawn objects at."), SerializeField]
    List<Vector3> spawnPositions = new();

    public FloatCounter Seconds;
    public Dictionary<GameObject, int> spawnedObjects = new();

    public void OnAwake() => Seconds = new(0, 0, interval, resetToMax: false);

    public void OnUpdate()
    {
        Seconds.Increase(Time.deltaTime);
        if (!Seconds.Exceeded) return;

        SpawnPickup();
        Seconds.Reset();
    }
    
    List<int> FreePositionIndices()
    {
        List<int> freePositionIndices = new();
        for (int i = 0; i < spawnPositions.Count; i++)
        {
            if (!spawnedObjects.Values.Contains(i))
                freePositionIndices.Add(i);
        }
        return freePositionIndices;
    }
    
    public void SpawnPickup()
    {
        List<int> freeIndices = FreePositionIndices();
        if (freeIndices.Count == 0) return;

        int randomFreeIndex = freeIndices[Random.Range(0, freeIndices.Count)];

        GameObject prefabToSpawn = mode switch
        {
            Mode.SpecificPrefab => prefab,
            Mode.RandomPrefabFromSet => set.TryGetRandomPrefab(out GameObject randomPrefab) ? randomPrefab : null,
            _ => null,
        };

        if (prefabToSpawn == null)
        {
            Debug.LogError($"{nameof(ObjectSpawnData)} received a null prefab!");
            return;
        }

        GameObject newObject = Object.Instantiate(prefabToSpawn, spawnPositions[randomFreeIndex], Quaternion.identity);

        if (!newObject.TryGetComponent(out IDespawnable despawnable))
        {
            Debug.LogError($"{nameof(ObjectSpawnData)} was configured with a prefab ({newObject.name}) that is missing {nameof(IDespawnable)}! Destroying...");
            Object.Destroy(newObject);
            return;
        }

        spawnedObjects[newObject] = randomFreeIndex;
        despawnable.OnDespawned += OnObjectDespawned;
    }

    void OnObjectDespawned(GameObject despawningObject)
    {
        if (!despawningObject.TryGetComponent(out IDespawnable despawnable))
        {
            Debug.LogError($"{nameof(ObjectSpawnData)} was configured with a prefab ({despawningObject.name}) that is missing {nameof(IDespawnable)}! Destroying...");
            Object.Destroy(despawningObject);
            return;
        }

        spawnedObjects.Remove(despawningObject);
        despawnable.OnDespawned -= OnObjectDespawned;
    }
}