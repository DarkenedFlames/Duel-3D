using System.Collections.Generic;
using System.Linq;
using HBM.Scriptable;
using UnityEngine;

public abstract class SpawnPointSet<T, TSpawnPoint> : RuntimeSet<TSpawnPoint> 
    where T : Component
    where TSpawnPoint : SpawnPoint<T>
{
    protected List<TSpawnPoint> FreePoints => this.Where(p => p != null && p.IsFree).ToList();

    /* Nice but keeps printing warnings in edit mode
    protected virtual void Awake()
    {
        if (this.Count() == 0)
            Debug.LogWarning($"{name} has no spawn point elements registered.");
    }
    */

    public void SpawnObject(GameObject prefab)
    {
        List<TSpawnPoint> freePoints = FreePoints;
        
        if (freePoints.Count == 0) return;

        TSpawnPoint point = freePoints[Random.Range(0, freePoints.Count)];
        point.AssignObject(Instantiate(prefab, point.transform.position, point.transform.rotation));
    }
}
