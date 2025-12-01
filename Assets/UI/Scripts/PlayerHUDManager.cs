using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{
    ResourcePanelUI resourcePanel;
    AbilityBarUI abilityBar;
    EffectBarUI effectBar;

    void Start()
    {
        resourcePanel = GetComponentInChildren<ResourcePanelUI>(true);
        abilityBar = GetComponentInChildren<AbilityBarUI>(true);
        effectBar = GetComponentInChildren<EffectBarUI>(true);

        var localPlayer = FindLocalPlayer();
        if (localPlayer == null)
        {
            Debug.LogError("No local player found for HUD initialization.");
            return;
        }

        if (localPlayer.TryGetComponent(out CharacterResources resources))
            resourcePanel.SubscribeToHandler(resources);

        if (localPlayer.TryGetComponent(out CharacterAbilities abilities))
            abilityBar.SubscribeToHandler(abilities);

        if (localPlayer.TryGetComponent(out CharacterEffects effects))
            effectBar.SubscribeToHandler(effects);
    }

    private GameObject FindLocalPlayer()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        return players.Length > 0 ? players[0] : null;
    }
}
