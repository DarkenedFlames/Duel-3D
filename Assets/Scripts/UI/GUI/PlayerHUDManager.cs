using System.Linq;
using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] CharacterSet allCharacters;
    [SerializeField] GameSessionSettings settings;
    
    AbilityBarUI abilityBar;
    WeaponSlotUI weaponSlot;
    StatPanelUI statPanel;
    PauseMenuUI pauseMenu;
    SettingsMenuUI settingsMenu;
    GameOverScreenUI gameOverScreen;
    
    Character localPlayer;
    FloatCounter sessionTimer;
    bool gameOver;

    void Awake()
    {
        abilityBar = GetComponentInChildren<AbilityBarUI>(true);
        weaponSlot = GetComponentInChildren<WeaponSlotUI>(true);
        statPanel = GetComponentInChildren<StatPanelUI>(true);
        pauseMenu = GetComponentInChildren<PauseMenuUI>(true);
        settingsMenu = GetComponentInChildren<SettingsMenuUI>(true);
        gameOverScreen = GetComponentInChildren<GameOverScreenUI>(true);
        
        if (settings != null && settings.Duration > 0)
            sessionTimer = new(settings.Duration, 0, settings.Duration);
    }
    
    void Start()
    {
        if (allCharacters == null || !allCharacters.TryGetSinglePlayer(out localPlayer))
        {
            Debug.LogError("No local player found for HUD initialization.");
            return;
        }

        abilityBar.Initialize(localPlayer);
        weaponSlot.SubscribeToHandler(localPlayer.CharacterWeapons);
        statPanel.SubscribeToHandler(localPlayer);
        pauseMenu.Initialize(localPlayer, settingsMenu);
        settingsMenu.Initialize(localPlayer, pauseMenu);
        
        // Subscribe to menu input
        localPlayer.CharacterInput.OnMenuInput += TogglePauseMenu;
        
        // Start with menus inactive
        pauseMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
        gameOverScreen.gameObject.SetActive(false);
    }
    
    void TogglePauseMenu()
    {
        if (pauseMenu != null)
            pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
    }
    
    void Update()
    {
        if (gameOver) return;
        
        sessionTimer?.Decrease(Time.deltaTime);
        if (sessionTimer != null && sessionTimer.Expired)
        {
            EndGame();
            return;
        }
        
        if (!allCharacters.TryGetSinglePlayer(out _))
        {
            EndGame();
            return;
        }
        
        if (allCharacters.Count() == 1)
        {
            EndGame(allCharacters.ToList()[0]);
            return;
        }
    }
    
    void EndGame(Character winner = null)
    {
        gameOver = true;
        gameOverScreen.gameObject.SetActive(true);
        gameOverScreen.Initialize(winner);
    }
    
    void OnDestroy()
    {
        if (localPlayer != null)
            localPlayer.CharacterInput.OnMenuInput -= TogglePauseMenu;
    }
}
