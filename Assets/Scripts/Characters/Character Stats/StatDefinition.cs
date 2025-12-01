using UnityEngine;

[CreateAssetMenu]
public class StatDefinition : ScriptableObject
{
    public string statName;
    public float defaultValue;
    public float RegenerationCooldown = 7f;
    public float RegenerationPercentage = 0.14f;
}