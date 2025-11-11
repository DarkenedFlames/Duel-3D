using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Health,
    Stamina,
    Mana,
    Speed
}

public class StatsHandler : MonoBehaviour
{
    private Dictionary<StatType, (float, float)> _values = new();
    public event Action<StatType, float, float> OnStatChanged;

    void Awake()
    {
        // Initialize all stats
        _values[StatType.Health]  = (100f, 100f);
        _values[StatType.Mana]    = (100f, 100f);
        _values[StatType.Stamina] = (100f, 100f);
        _values[StatType.Speed]   = (100f, 100f);
    }

    void Update()
    {
        (float current, float _) = _values[StatType.Health];
        if (current <= 0f) Die();
    }

    public float GetStat(StatType type, bool getMax)
    {
        (float current, float max) = _values[type];
        return getMax ? max : current;
    }

    public bool TrySetStat(StatType type, bool setMax, float newVal)
    {
        (float current, float max) = _values[type];
        float oldVal = setMax ? max : current;

        if (setMax)
        {
            float newMax = newVal;
            float newCurrent = Mathf.Clamp(current, 0, newMax);
            _values[type] = (newCurrent, newMax);
        }
        else
        {
            float newCurrent = Mathf.Clamp(newVal, 0, max);
            _values[type] = (newCurrent, max);
        }

        // Only trigger event if it changed
        if (Mathf.Approximately(newVal, oldVal)) 
            return false;

        (float finalCurrent, float finalMax) = _values[type];
        Debug.Log($"{gameObject.name}'s {(setMax ? "maximum" : "")} {type} changed from {oldVal} to {newVal}!");
        OnStatChanged?.Invoke(type, finalCurrent, finalMax);

        return true;
    }


    public bool TryModifyStat(StatType type, bool modifyMax, float delta)
    {
        (float current, float max) = _values[type];
        float oldVal = modifyMax ? max : current;
        return TrySetStat(type, setMax: modifyMax, oldVal + delta);
    }

    public void RefillStat(StatType type)
    {
        (float _, float max) = _values[type];
        _values[type] = (max, max);
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0) return;
        (float current, float _) = _values[StatType.Health];
        TrySetStat(StatType.Health, setMax: false, current - amount);
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }

}