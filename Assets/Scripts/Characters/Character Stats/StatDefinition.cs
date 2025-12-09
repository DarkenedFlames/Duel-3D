using UnityEngine;

public enum StatType 
{ 
    // Core Combat Stats
    MaxHealth, 
    MaxMana, 
    MaxStamina,
    Attack, 
    Defense, 
    Armor, 
    Shield,
    CriticalChance,
    CriticalDamage,
    
    // Movement & Utility
    MovementSpeed,
    AttackSpeed,
    CooldownReduction,
    
    // Add more as needed
}

[CreateAssetMenu]
public class StatDefinition : ScriptableObject
{
    public string statName;
    public StatType statType;
    public float defaultValue;
}