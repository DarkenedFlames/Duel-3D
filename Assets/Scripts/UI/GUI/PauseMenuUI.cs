using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    PlayerInputDriver input;
    SettingsMenuUI settingsMenu;

    public void Initialize(Character player, SettingsMenuUI settings)
    {
        input = player.GetComponent<PlayerInputDriver>();
        settingsMenu = settings;
    }
    
    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        if (input != null) input.enabled = false;
    }
    
    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        if (input != null) input.enabled = true;
    }

    public void ExitMenu()
    {
        gameObject.SetActive(false);
    }

    public void OpenSettingsMenu()
    {
        gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}