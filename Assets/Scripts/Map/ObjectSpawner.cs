using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Tooltip("Select the prefab and spawn interval for this set of points."), SerializeField]
    List<ObjectSpawnData> data = new();

    void Awake() => data.ForEach(d => d.OnAwake());
    void Update() => data.ForEach(d => d.OnUpdate());
    void OnDrawGizmos() => data.ForEach(d => d.DrawGizmos());
}