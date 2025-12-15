using HBM.Scriptable;
using UnityEngine;

public abstract class SpawnPointRegistrarBase<T, TSpawnPoint> : SetRegistarBase<TSpawnPoint>
    where T : Component
    where TSpawnPoint : SpawnPoint<T>
{
    protected override TSpawnPoint _object => GetComponent<TSpawnPoint>();
}
