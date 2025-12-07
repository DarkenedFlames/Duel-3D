using System.Linq;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] GameObject pickupPrefab;
    [SerializeField] float spawnInterval = 5f;
    [SerializeField] PickupSpawnPoint[] spawnPoints;

    FloatCounter timer;

    void Start() => timer = new(0, 0, spawnInterval, resetToMax: false);
    void Update()
    {
        timer.Increase(Time.deltaTime);
        if (timer.Exceeded)
        {
            SpawnPickup();
            timer.Reset();
        }
    }

    void SpawnPickup()
    {
        PickupSpawnPoint[] freePoints = spawnPoints.Where(p => p.IsFree).ToArray();
        if (freePoints.Length == 0) return;

        PickupSpawnPoint point = freePoints[Random.Range(0, freePoints.Length)];
        point.AssignPickup(Instantiate(pickupPrefab, point.transform.position, point.transform.rotation));
    }
}