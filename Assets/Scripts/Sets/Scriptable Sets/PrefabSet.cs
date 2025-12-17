using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Prefab Set", menuName = "Scriptable Definition Sets/Prefab")]

public class PrefabSet : ScriptableObject
{
    public List<GameObject> Prefabs = new();

    public bool TryGetRandomPrefab(out GameObject prefab)
    {
        prefab = null;

        if (Prefabs.Count > 0)
            prefab = Prefabs[Random.Range(0, Prefabs.Count)];
        
        return prefab != null;
    }
}