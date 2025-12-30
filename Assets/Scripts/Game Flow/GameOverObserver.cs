using System.Linq;
using UnityEngine;

public class GameOverObserver : MonoBehaviour
{
    [SerializeField] CharacterSet allCharacters;
    [SerializeField] GameOverScreenUI gameOverScreenUI;
    [SerializeField] GameSessionSettings settings;

    FloatCounter seconds;
    bool gameOver;

    void Awake()
    {
        if (settings.Duration > 0)
            seconds = new(settings.Duration, 0, settings.Duration);

    }
    
    void Start()
    {
        if (allCharacters == null)
            Debug.LogError($"{nameof(GameOverObserver)} was configured with a null or empty {nameof(CharacterSet)}!");
    }
    

    private void Update()
    {
        if (gameOver) return;

        seconds?.Decrease(Time.deltaTime);
        if (seconds != null && seconds.Expired)
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
        Instantiate(gameOverScreenUI).Initialize(winner);
    }
}