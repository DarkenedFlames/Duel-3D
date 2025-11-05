using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Projectile Definition", menuName = "Projectiles/New Definition")]
public class ProjectileDefinition : ScriptableObject
{
    [Header("Prefab")]
    public GameObject projectilePrefab;
    public string projectileName;
    //public string animationTrigger = "AbilityTrigger";

    [Header("Collision")]
    public bool collidesWithSource;
    public bool collidesWithTerrain;

    [Header("Behaviors")]
    public List<ProjectileBehaviorDefinition> behaviors = new();

}