using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] CharacterSet allCharacters;
    AbilityBarUI abilityBar;
    WeaponSlotUI weaponSlot;
    StatPanelUI statPanel;

    void Start()
    {
        abilityBar = GetComponentInChildren<AbilityBarUI>(true);
        weaponSlot = GetComponentInChildren<WeaponSlotUI>(true);
        statPanel = GetComponentInChildren<StatPanelUI>(true);

        if (allCharacters == null || !allCharacters.TryGetSinglePlayer(out Character localPlayer))
        {
            Debug.LogError("No local player found for HUD initialization.");
            return;
        }

        abilityBar.Initialize(localPlayer);
        weaponSlot.SubscribeToHandler(localPlayer.CharacterWeapons);
        statPanel.SubscribeToHandler(localPlayer);
    }
}
