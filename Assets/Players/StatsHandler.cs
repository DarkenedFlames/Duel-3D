using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum StatType
{
    Health,
    MaxHealth,
    Stamina,
    MaxStamina,
    Mana,
    MaxMana,
    Speed
}

public class StatsHandler : MonoBehaviour
{
    [Header("Sliders")]
    [Tooltip("Do not add for AI players")]
    [SerializeField] ResourceBarUI healthBarUI;

    [Tooltip("Do not add for AI players")]
    [SerializeField] ResourceBarUI staminaBarUI;

    [Tooltip("Do not add for AI players")]
    [SerializeField] ResourceBarUI manaBarUI;

    readonly Dictionary<StatType, float> _values = new();

    void Awake()
    {
        // Initialize all stats to 100 by default
        foreach (StatType stat in Enum.GetValues(typeof(StatType)))
            _values[stat] = 100f;

        // Ensure current values match their max
        RefillStat(StatType.Health, StatType.MaxHealth);
        RefillStat(StatType.Stamina, StatType.MaxStamina);
        RefillStat(StatType.Mana, StatType.MaxMana);
    }

    void Update()
    {
        // Clamp current stats to their max limits
        ClampStat(StatType.Health, StatType.MaxHealth);
        ClampStat(StatType.Stamina, StatType.MaxStamina);
        ClampStat(StatType.Mana, StatType.MaxMana);

        // Check death
        if (_values[StatType.Health] <= 0f)
            Die();

        UpdateUI();
    }

    public bool TryGetStat(StatType type, out float val) => _values.TryGetValue(type, out val);
    
    public bool TrySetStat(StatType type, float newVal)
    {
        if (!_values.ContainsKey(type)) return false;

        float oldVal = _values[type];
        _values[type] = newVal;

        if (newVal != oldVal)
            Debug.Log($"{gameObject.name}'s {type} changed from {oldVal} to {newVal}!");
        return true;
    }

    bool ClampStat(StatType type, StatType maxType)
    {
        if (!TryGetStat(type, out float current) || !TryGetStat(maxType, out float maxVal))
            return false;

        float clamped = Mathf.Clamp(current, 0f, maxVal);
        return TrySetStat(type, clamped);
    }

    public bool TryDecreaseStat(StatType type, float amount)
    {
        if (amount <= 0f)
            return false;

        if (!TryGetStat(type, out float current))
            return false;

        float newVal = Mathf.Max(0f, current - amount);
        if (!TrySetStat(type, newVal))
            return false;

        Debug.Log($"{gameObject.name}'s {type} decreased by {amount}!");
        return true;
    }

    public bool RefillStat(StatType type, StatType maxType)
    {
        if (!TryGetStat(maxType, out float maxVal))
            return false;

        return TrySetStat(type, maxVal);
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0) return;
        TryDecreaseStat(StatType.Health, amount);
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }

    void UpdateUI()
    {
        
        if (healthBarUI != null)
            healthBarUI.SetValue(_values[StatType.Health], _values[StatType.MaxHealth]);

        if (staminaBarUI != null)
            staminaBarUI.SetValue(_values[StatType.Stamina], _values[StatType.MaxStamina]);

        if (manaBarUI != null)
            manaBarUI.SetValue(_values[StatType.Mana], _values[StatType.MaxMana]);
    }
}