using UnityEngine;

public class CharacterWorldUI : MonoBehaviour
{
    ResourcePanelUI resourcePanel;
    EffectBarUI effectBar;
    NameBarUI nameBar;

    Vector3 offset = Vector3.zero;
    Character owner;

    void Awake()
    {
        resourcePanel = GetComponentInChildren<ResourcePanelUI>();
        effectBar = GetComponentInChildren<EffectBarUI>();
        nameBar = GetComponentInChildren<NameBarUI>();
    }

    void LateUpdate()
    {
        if (owner == null) return;

        transform.SetPositionAndRotation(
            owner.transform.position + offset,
            Quaternion.LookRotation(transform.position - Camera.main.transform.position)
        );
    }

    public void Initialize(Character character, Vector3 offset)
    {
        this.offset = offset;
        owner = character;

        owner.CharacterResources.EnsureInitialized();
        
        if (resourcePanel != null)
            resourcePanel.SubscribeToHandler(owner.CharacterResources);
        
        if (effectBar != null)
            effectBar.SubscribeToHandler(owner.CharacterEffects);

        if (nameBar != null)
            nameBar.Initialize(owner);
    }
}
