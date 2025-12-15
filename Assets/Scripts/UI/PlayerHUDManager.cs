using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] CharacterSet allCharacters;
    AbilityBarUI abilityBar;
    WeaponSlotUI weaponSlot;

    void Start()
    {
        abilityBar = GetComponentInChildren<AbilityBarUI>(true);
        weaponSlot = GetComponentInChildren<WeaponSlotUI>(true);

        if (allCharacters == null || !allCharacters.TryGetSinglePlayer(out Character localPlayer))
        {
            Debug.LogError("No local player found for HUD initialization.");
            return;
        }

        abilityBar.SubscribeToHandler(localPlayer.CharacterAbilities);
        weaponSlot.SubscribeToHandler(localPlayer.CharacterWeapons);
    }
}
