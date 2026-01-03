using UnityEngine;

public class CharacterWorldUI : MonoBehaviour
{
    [SerializeField] ResourcePanelUI resourcePanel;
    [SerializeField] EffectBarUI effectBar;
    [SerializeField] NameBarUI nameBar;

    Vector3 offset = Vector3.zero;
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
        
        resourcePanel.SubscribeToHandler(owner.CharacterResources);
        effectBar.SubscribeToHandler(owner.CharacterEffects);
        nameBar.Initialize(owner);
    }
}
