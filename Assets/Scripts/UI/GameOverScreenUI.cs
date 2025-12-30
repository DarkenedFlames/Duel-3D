using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreenUI : MonoBehaviour
{
    [SerializeField] MainMenuUI mainMenuUI;
    [SerializeField] TextMeshProUGUI winnerText;

    public void Initialize(Character winner = null)
    {
        winnerText.text = winner == null
            ? $"Draw!"
            : $"Winner: {winner.gameObject.name}!";

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Button hooks
    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void ReturnToMain() => SceneManager.LoadScene(0);

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}