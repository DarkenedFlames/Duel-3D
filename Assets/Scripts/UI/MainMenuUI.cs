using UnityEngine;

public class MainMenuUI : MonoBehaviour
{   
    [SerializeField] GameSetupUI gameSetupUI;
    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Button hooks
    public void PlayGame()
    {
        Instantiate(gameSetupUI, transform.root);
        Destroy(gameObject);
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