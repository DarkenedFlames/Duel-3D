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

        _effects.OnEffectGained += HandleEffectGained;
        _effects.OnEffectLost += HandleEffectLost;

        _effects.OnEffectStackChanged += HandleEffectStackChanged;
        _effects.OnEffectRefreshed += HandleEffectRefreshed;
        _effects.OnEffectExtended += HandleEffectExtended;

        _effects.OnEffectMaxStacksReached += HandleMaxStacksReached;
        _effects.OnEffectPulsed += HandleEffectPulsed;

        foreach (var effect in _effects.Effects)
            HandleEffectGained(effect);
    }

    private void OnDestroy()
    {
        if (_effects == null) return;

        _effects.OnEffectGained -= HandleEffectGained;
        _effects.OnEffectLost -= HandleEffectLost;

        _effects.OnEffectStackChanged -= HandleEffectStackChanged;
        _effects.OnEffectRefreshed -= HandleEffectRefreshed;
        _effects.OnEffectExtended -= HandleEffectExtended;

        _effects.OnEffectMaxStacksReached -= HandleMaxStacksReached;
        _effects.OnEffectPulsed -= HandleEffectPulsed;
    }

    private void HandleEffectGained(CharacterEffect effect)
    {
        if (activeSlots.ContainsKey(effect)) return;

        var slot = Instantiate(effectSlotPrefab, layoutGroup.transform);
        slot.Initialize(effect);
        activeSlots.Add(effect, slot);
    }

    private void HandleEffectLost(CharacterEffect effect)
    {
        if (activeSlots.TryGetValue(effect, out var slot))
        {
            Destroy(slot.gameObject);
            activeSlots.Remove(effect);
        }
    }

    private void HandleEffectStackChanged(CharacterEffect effect)
    {
        if (activeSlots.TryGetValue(effect, out var slot))
            slot.UpdateStackCount();
    }

    private void HandleEffectRefreshed(CharacterEffect effect)
    {
        if (activeSlots.TryGetValue(effect, out var slot))
            slot.UpdateDuration();
    }

    private void HandleEffectExtended(CharacterEffect effect)
    {
        if (activeSlots.TryGetValue(effect, out var slot))
            slot.UpdateDuration();
    }

    private void HandleMaxStacksReached(CharacterEffect effect)
    {
        if (activeSlots.TryGetValue(effect, out var slot))
            slot.PlayMaxStackEffect();   // Placeholder
    }

    private void HandleEffectPulsed(CharacterEffect effect)
    {
        if (activeSlots.TryGetValue(effect, out var slot))
            slot.PlayPulseEffect();       // Placeholder
    }
}
