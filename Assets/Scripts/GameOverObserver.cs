using System.Linq;
using UnityEngine;

public class GameOverObserver : MonoBehaviour
{
    [SerializeField] CharacterSet allCharacters;
    [SerializeField] GameOverScreenUI uiPrefab;

    bool gameOver;

    private void Update()
    {
        Character player = null;
        gameOver = allCharacters != null
            && allCharacters.Count() == 1
            && allCharacters.TryGetSinglePlayer(out player);
       
        if (!gameOver) return;
        
        Instantiate(uiPrefab).Initialize(player);
        enabled = false;
    }
}