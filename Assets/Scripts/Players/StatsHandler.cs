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
    private Dictionary<StatType, (float current, float max)> _values = new();
    public event Action<StatType, float, float> OnStatChanged;

    private void Awake()
    {
        foreach (StatType type in Enum.GetValues(typeof(StatType)))
            _values[type] = (0f, 0f);
    }

    private void Start()
    {
        SetDefaultStat(StatType.Health, 100f);
        SetDefaultStat(StatType.Stamina, 100f);
        SetDefaultStat(StatType.Mana, 100f);
        SetDefaultStat(StatType.Speed, 100f);
    }

    private void SetDefaultStat(StatType type, float value)
    {
        TrySetStat(type, setMax: true, value);
        TrySetStat(type, setMax: false, value);
    }


    private void Update()
    {
        (float current, _) = _values[StatType.Health];
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

        if (Mathf.Approximately(newVal, oldVal))
            return false;

        (float finalCurrent, float finalMax) = _values[type];
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
        TrySetStat(type, setMax: false, max);
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0) return;
        (float current, _) = _values[StatType.Health];
        TrySetStat(StatType.Health, setMax: false, current - amount);
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }
}
