using UnityEngine;

public class CharacterInterface : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject worldUIPrefab;
    
    [Header("Settings")]
    [SerializeField] Vector3 uiOffset = new(0, 2.5f, 0);

    Character owner;
    CharacterWorldUI spawnedUI;

    void Start()
    {
        owner = GetComponent<Character>();
        spawnedUI = Instantiate(worldUIPrefab).GetComponent<CharacterWorldUI>();
        spawnedUI.Initialize(owner, uiOffset);
    }

    void OnDestroy()
    {
        if (spawnedUI != null)
            Destroy(spawnedUI.gameObject);
    }
}
