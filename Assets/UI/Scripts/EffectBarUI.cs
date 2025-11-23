
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class EffectBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EffectSlotUI effectSlotPrefab;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
    private CharacterEffects _effects;

    private readonly Dictionary<CharacterEffect, EffectSlotUI> activeSlots = new();

    private void Awake()
    {
        if (layoutGroup == null)
            layoutGroup = GetComponent<HorizontalLayoutGroup>();
    }

    public void SubscribeToHandler(CharacterEffects effects)
    {
        _effects = effects;

        _effects.OnEffectGained += HandleEffectApplied;
        _effects.OnEffectLost += HandleEffectExpired;
        _effects.OnEffectStackChanged += HandleEffectStackChange;
        _effects.OnEffectRefreshed += HandleEffectRefreshed;

        foreach (CharacterEffect effect in _effects.Effects)
            HandleEffectApplied(effect);
    }

    void OnDestroy()
    {
        _effects.OnEffectGained -= HandleEffectApplied;
        _effects.OnEffectLost -= HandleEffectExpired;
        _effects.OnEffectStackChanged -= HandleEffectStackChange;
        _effects.OnEffectRefreshed -= HandleEffectRefreshed;
    }

    private void HandleEffectApplied(CharacterEffect effect)
    {
        if (activeSlots.ContainsKey(effect)) return;

        EffectSlotUI newSlot = Instantiate(effectSlotPrefab, layoutGroup.transform);
        newSlot.Initialize(effect);
        activeSlots.Add(effect, newSlot);
    }

    private void HandleEffectExpired(CharacterEffect effect)
    {
        if (activeSlots.TryGetValue(effect, out var slot))
        {
            Destroy(slot.gameObject);
            activeSlots.Remove(effect);
        }
    }

    private void HandleEffectStackChange(CharacterEffect effect)
    {
        if (activeSlots.TryGetValue(effect, out var slot))
            slot.UpdateStackCount(effect.currentStacks.Value);
    }

    private void HandleEffectRefreshed(CharacterEffect effect)
    {
        if (activeSlots.TryGetValue(effect, out var slot))
            slot.RefreshDuration(effect.seconds.Value, effect.Definition.duration);
    }
}
