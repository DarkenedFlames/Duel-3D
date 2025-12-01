using UnityEngine;

[CreateAssetMenu]
public class ResourceDefinition : ScriptableObject
{
    public string ResourceName = "Untitled Resource";
    public StatDefinition MaxStat;
    public float RegenerationCooldown = 7f;
    public float RegenerationPercentage = 0.14f;

}