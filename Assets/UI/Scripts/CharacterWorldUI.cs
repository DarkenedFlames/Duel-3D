using UnityEngine;

public class CharacterWorldUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] ResourcePanelUI resourcePanel;
    [SerializeField] EffectBarUI effectBar;

    Vector3 offset = new(0, 2.5f, 0);
    Character owner;

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
    }
}
