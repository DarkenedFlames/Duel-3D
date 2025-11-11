using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class EffectBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EffectSlotUI effectSlotPrefab;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
    private EffectHandler effectHandler;

    private readonly Dictionary<Effect, EffectSlotUI> activeSlots = new();

    private void Awake()
    {
        if (layoutGroup == null)
            layoutGroup = GetComponent<HorizontalLayoutGroup>();
    }

    public void SubscribeToHandler(EffectHandler handler)
    {
        if (handler == null) return;
        effectHandler = handler;

        effectHandler.OnEffectApplied += HandleEffectApplied;
        effectHandler.OnEffectExpired += HandleEffectExpired;
        effectHandler.OnEffectStackChange += HandleEffectStackChange;
        effectHandler.OnEffectRefreshed += HandleEffectRefreshed;
    }

    void OnDestroy()
    {
        effectHandler.OnEffectApplied -= HandleEffectApplied;
        effectHandler.OnEffectExpired -= HandleEffectExpired;
        effectHandler.OnEffectStackChange -= HandleEffectStackChange;
        effectHandler.OnEffectRefreshed -= HandleEffectRefreshed;
    }

    private void HandleEffectApplied(Effect effect)
    {
        if (activeSlots.ContainsKey(effect)) return;

        EffectSlotUI newSlot = Instantiate(effectSlotPrefab, layoutGroup.transform);
        newSlot.Initialize(effect);
        activeSlots.Add(effect, newSlot);
    }

    private void HandleEffectExpired(Effect effect)
    {
        if (activeSlots.TryGetValue(effect, out var slot))
        {
            Destroy(slot.gameObject);
            activeSlots.Remove(effect);
        }
    }

    private void HandleEffectStackChange(Effect effect)
    {
        if (activeSlots.TryGetValue(effect, out var slot))
            slot.UpdateStackCount(effect.CurrentStacks);
    }

    private void HandleEffectRefreshed(Effect effect)
    {
        if (activeSlots.TryGetValue(effect, out var slot))
            slot.RefreshDuration(effect.RemainingTime(), effect.Definition.duration);
    }
}
