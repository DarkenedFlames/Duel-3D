
using System;
using UnityEngine;

public interface IDespawnable
{
    event Action<GameObject> OnDespawned;
}