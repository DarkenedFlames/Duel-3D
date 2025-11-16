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

[RequireComponent(typeof(AbilityHandler))]
public class StatsHandler : MonoBehaviour
{
    private Dictionary<StatType, (float current, float max)> _values = new();
    public event Action<StatType, float, float> OnStatChanged;
    public event Action<GameObject> OnDeath;

    private AbilityHandler abilities;

    private void Awake()
    {
        foreach (StatType type in Enum.GetValues(typeof(StatType)))
            _values[type] = (0f, 0f);

        abilities = GetComponent<AbilityHandler>();

    }

    // UI Relies on the OnStatChanged invocations from this. Keep in Start().
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

    void OnEnable()
    {
        abilities.OnAbilityActivated += HandleAbilityActivated;
    }

    void Osable()
    {
        abilities.OnAbilityActivated -= HandleAbilityActivated;
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

        Debug.Log($"{gameObject.name}'s {type} changed from {oldVal} to {newVal}");

        (float finalCurrent, float finalMax) = _values[type];
        OnStatChanged?.Invoke(type, finalCurrent, finalMax);
        return true;
    }

    public bool TryModifyStat(StatType type, bool modifyMax, float delta)
    {
        (float current, float max) = _values[type];
        float oldVal = modifyMax ? max : current;
        float newVal = oldVal + delta;

        return TrySetStat(type, setMax: modifyMax, newVal);
    }

    public void RefillStat(StatType type) =>
        TrySetStat(type, setMax: false, GetStat(type, getMax: true));
    

    public void TakeDamage(float amount)
    {
        if (amount <= 0) return;
        TryModifyStat(StatType.Health, modifyMax: false, -1f * amount);
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        OnDeath?.Invoke(gameObject);
        Destroy(gameObject);
    }

    private void HandleAbilityActivated(Ability ability) =>
        TryModifyStat(StatType.Mana, modifyMax: false, -1f * ability.Definition.manaCost);
    
}
