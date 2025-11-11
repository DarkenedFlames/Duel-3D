using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{
    private ResourceBarUI[] resourceBars;
    private AbilityBarUI abilityBar;
    private EffectBarUI effectBar;

    private StatsHandler playerStats;
    private AbilityHandler abilityHandler;
    private EffectHandler effectHandler;

    void Awake()
    {
        resourceBars = GetComponentsInChildren<ResourceBarUI>(true);
        abilityBar = GetComponentInChildren<AbilityBarUI>(true);
        effectBar = GetComponentInChildren<EffectBarUI>(true);

        // Find local player early so we can subscribe to abilities before Awake of AbilityHandler finishes
        GameObject localPlayer = FindLocalPlayer();
        if (localPlayer == null)
        {
            Debug.LogError("No local player found for HUD initialization.");
            return;
        }
        
        // Subscribe early, abilities are set in Start
        if (localPlayer.TryGetComponent(out abilityHandler))
            abilityBar.SubscribeToHandler(abilityHandler);

        if (localPlayer.TryGetComponent(out effectHandler))
            effectBar.SubscribeToHandler(effectHandler);
    }

    // Subscribe late, stats are set in Awake
    void Start()
    {
        GameObject localPlayer = FindLocalPlayer();
        if (localPlayer.TryGetComponent(out playerStats))
        {
            foreach (ResourceBarUI bar in resourceBars)
                bar.Initialize(playerStats);
        }
    }

    private GameObject FindLocalPlayer()
    {
        // TODO: Replace with network-local player lookup in multiplayer
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        return players.Length > 0 ? players[0] : null;
    }
}
