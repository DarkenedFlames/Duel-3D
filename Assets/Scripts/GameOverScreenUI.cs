using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreenUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI winnerText;

    public void Initialize(Character winner)
    {
        winnerText.text = $"Winner: {winner.gameObject.name}";

        PauseGame();
        UnlockCursor();
    }

    void PauseGame() => Time.timeScale = 0f;
    
    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Button hooks
    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Reset Button Clicked");
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