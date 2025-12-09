using UnityEngine;

public enum ResourceType
{
    Health,
    Mana,
    Stamina,
}

[CreateAssetMenu]
public class ResourceDefinition : ScriptableObject
{
    public string ResourceName = "Untitled Resource";
    public ResourceType resourceType;
    public StatDefinition MaxStat;
    public float RegenerationCooldown = 7f;
    public float RegenerationPercentage = 0.14f;

}