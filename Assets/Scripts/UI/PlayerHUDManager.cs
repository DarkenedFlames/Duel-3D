using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{
    private ResourceBarUI[] resourceBars;
    private AbilityBarUI abilityBar;
    private EffectBarUI effectBar;

    private void Awake()
    {
        resourceBars = GetComponentsInChildren<ResourceBarUI>(true);
        abilityBar = GetComponentInChildren<AbilityBarUI>(true);
        effectBar = GetComponentInChildren<EffectBarUI>(true);

        var localPlayer = FindLocalPlayer();
        if (localPlayer == null)
        {
            Debug.LogError("No local player found for HUD initialization.");
            return;
        }

        if (localPlayer.TryGetComponent(out StatsHandler stats))
            foreach (var bar in resourceBars)
                bar.SubscribeToHandler(stats); // subscribes to OnStatChanged

        if (localPlayer.TryGetComponent(out AbilityHandler abilities))
            abilityBar.SubscribeToHandler(abilities);

        if (localPlayer.TryGetComponent(out EffectHandler effects))
            effectBar.SubscribeToHandler(effects);
    }

    private GameObject FindLocalPlayer()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        return players.Length > 0 ? players[0] : null;
    }
}
