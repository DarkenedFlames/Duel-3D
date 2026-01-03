using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class EffectBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] EffectSlotUI effectSlotPrefab;
    [SerializeField] HorizontalLayoutGroup layoutGroup;

    CharacterEffects effects;
    readonly List<EffectSlotUI> slotPool = new();

    bool TryGetSlotByEffect(CharacterEffect effect, out EffectSlotUI slot)
    {
        slot = slotPool.Find(s => s.Effect == effect);
        return slot != null;
    }

    public void SubscribeToHandler(CharacterEffects effects)
    {
        this.effects = effects;

        effects.OnEffectGained += HandleEffectGained;
        effects.OnEffectLost += HandleEffectLost;
        effects.OnEffectStackChanged += HandleEffectStackChanged;
    }

    void OnDestroy()
    {
        effects.OnEffectGained -= HandleEffectGained;
        effects.OnEffectLost -= HandleEffectLost;
        effects.OnEffectStackChanged -= HandleEffectStackChanged;
    }

    void HandleEffectGained(CharacterEffect effect)
    {
        if (TryGetSlotByEffect(effect, out _)) return;

        EffectSlotUI slot = Instantiate(effectSlotPrefab, layoutGroup.transform);
        slot.Initialize(effect);
        slotPool.Add(slot);
    }

    void HandleEffectLost(CharacterEffect effect)
    {
        if (!TryGetSlotByEffect(effect, out EffectSlotUI slot)) return;
        
        slotPool.Remove(slot);
        Destroy(slot.gameObject);
    }

    void HandleEffectStackChanged(CharacterEffect effect)
    {
        if (!TryGetSlotByEffect(effect, out EffectSlotUI slot)) return;
        slot.UpdateStackCount();
    }
}
