using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class EffectHandler : MonoBehaviour
{
    public event Action<GameObject> OnEffectGained;
    public event Action<GameObject> OnEffectLost;
    public event Action<GameObject> OnEffectStackChanged;
    public event Action<GameObject> OnEffectRefreshed;

    public List<EffectData> GetEffectData() => GetComponentsInChildren<EffectData>().ToList();
    public List<int> GetEffectStacks() => GetEffectData().Select(d => d.currentStacks).ToList();
    public List<float> GetEffectTimes()
    {
        List<float> times = new();
        
        foreach (GameObject effect in GetEffects())
            times.Add(effect.GetComponent<Expiration>().Remaining());

        return times;
    } // might want to output a Dictionary<GameObject, float>, same with GetEffectStacks()

    public List<GameObject> GetEffects() => GetEffectData().Select(d => d.gameObject).ToList();
    public bool TryGetEffect(string effectName, out GameObject effect)
    {
        effect = GetEffects().FirstOrDefault(e => e.name == effectName);
        return effect != null;
    }

    public void ApplyEffect(GameObject prefab, int stacks = 1)
    {
        if (TryGetEffect(prefab.name, out GameObject existing))
        {
            EffectData existingData = existing.GetComponent<EffectData>();
            ApplyStacking(existingData, stacks);
            return;
        }

        GameObject newEffect = Instantiate(prefab, transform);
        EffectData newData = newEffect.GetComponent<EffectData>();

        newEffect.GetComponent<Expiration>().OnExpired += HandleExpiration;
        OnEffectGained?.Invoke(newEffect);
        ApplyStacking(newData, stacks - 1);  
    }

    void ApplyStacking(EffectData data, int stacksToAdd)
    {
        bool changed = false;

        if (data.stackingType.HasFlag(StackingType.Refresh))
        {
            data.GetComponent<Expiration>().Reset();
            changed = true;
            OnEffectRefreshed?.Invoke(data.gameObject);
        }

        // Add stacks?
        if (data.stackingType.HasFlag(StackingType.AddStack))
        {
            int before = data.currentStacks;

            data.currentStacks = Mathf.Min(
                data.currentStacks + stacksToAdd,
                data.maxStacks
            );

            changed |= data.currentStacks != before;
        }

        if (changed)
            OnEffectStackChanged?.Invoke(data.gameObject);
    }

    public void RemoveStacks(string effectName, int stacks = 1)
    {
        if (stacks <= 0) return;

        if (TryGetEffect(effectName, out GameObject effect))
        {
            EffectData data = effect.GetComponent<EffectData>();
            data.currentStacks = Mathf.Clamp(data.currentStacks - stacks, 0, data.maxStacks);

            if (data.currentStacks <= 0)
                CleanseEffect(effect);
        }
    }

    void HandleExpiration(Expiration exp)
    {
        EffectData data = exp.GetComponent<EffectData>();

        if (data.expiryType == ExpiryType.LoseOneStackAndRefresh)
        {
            if (data.currentStacks > 1)
            {
                data.currentStacks--;
                exp.Reset();

                OnEffectStackChanged?.Invoke(data.gameObject);
                return;
            }
        }

        CleanseEffect(data.gameObject);
    }

    void CleanseEffect(GameObject effect)
    {
        OnEffectLost?.Invoke(effect);
        Destroy(effect);
    }
}