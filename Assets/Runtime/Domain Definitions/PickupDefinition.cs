
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Pickup Definition", menuName = "Pickups/New Definition")]
public class PickupDefinition : ScriptableObject
{
    [Header("Prefab")]
    public GameObject pickupPrefab;

    [Header("Pickup Info")]
    public string pickupName;
    public Vector3 pickupSize = Vector3.one;

    [Header("Behaviors")]
    public List<PickupBehaviorDefinition> behaviors;
}

