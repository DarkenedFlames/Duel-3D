using UnityEngine;

public class IconHover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject hoverUIPrefab;
    [SerializeField] Sprite iconToHover;
    
    [Header("Settings")]
    [SerializeField] Vector3 uiOffset = new(0, 2.5f, 0);

    IconHoverUI spawnedUI;

    void Start()
    {
        spawnedUI = Instantiate(hoverUIPrefab).GetComponent<IconHoverUI>();
        spawnedUI.Initialize(transform, uiOffset, iconToHover);
    }

    void OnDestroy()
    {
        if (spawnedUI != null)
            Destroy(spawnedUI.gameObject);
    }
}