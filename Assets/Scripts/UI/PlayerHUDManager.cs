using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{
    AbilityBarUI abilityBar;
    WeaponSlotUI weaponSlot;

    void Start()
    {
        abilityBar = GetComponentInChildren<AbilityBarUI>(true);
        weaponSlot = GetComponentInChildren<WeaponSlotUI>(true);

        var localPlayer = FindLocalPlayer();
        if (localPlayer == null)
        {
            Debug.LogError("No local player found for HUD initialization.");
            return;
        }

        if (localPlayer.TryGetComponent(out CharacterAbilities abilities))
            abilityBar.SubscribeToHandler(abilities);
        if (localPlayer.TryGetComponent(out CharacterWeapons weapons))
            weaponSlot.SubscribeToHandler(weapons);
    }

    private GameObject FindLocalPlayer()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        return players.Length > 0 ? players[0] : null;
    }
}
