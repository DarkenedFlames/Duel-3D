using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public enum ShadowResolution { Low = 256, LowMedium = 512, Medium = 1024, HighMedium = 2048, High = 4096, Ultra = 8192 }
public enum ShadowDistance { None = 0, Low = 50, High = 150 }
public enum RenderScale { Low = 50, Medium = 75, High = 100 }
public class SettingsMenuUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] UniversalRenderPipelineAsset urpAsset;
    [SerializeField] TMP_Dropdown shadowResolutionDropdown;
    [SerializeField] TMP_Dropdown shadowDistanceDropdown;
    [SerializeField] TMP_Dropdown renderScaleDropdown;

    PauseMenuUI pauseMenu;
    PlayerInputDriver input;

    public void Initialize(Character player, PauseMenuUI pause)
    {
        input = player.GetComponent<PlayerInputDriver>();
        pauseMenu = pause;
        
        // Populate Shadow Resolution dropdown
        shadowResolutionDropdown.ClearOptions();
        shadowResolutionDropdown.AddOptions(Enum.GetNames(typeof(ShadowResolution)).ToList());
        shadowResolutionDropdown.onValueChanged.AddListener(SetShadowResolution);
        shadowResolutionDropdown.value = 0;

        // Populate Shadow Distance dropdown
        shadowDistanceDropdown.ClearOptions();
        shadowDistanceDropdown.AddOptions(Enum.GetNames(typeof(ShadowDistance)).ToList());
        shadowDistanceDropdown.onValueChanged.AddListener(SetShadowDistance);
        shadowDistanceDropdown.value = 0;

        // Populate Render Scale dropdown
        renderScaleDropdown.ClearOptions();
        renderScaleDropdown.AddOptions(Enum.GetNames(typeof(RenderScale)).ToList());
        renderScaleDropdown.onValueChanged.AddListener(SetRenderScale);
        renderScaleDropdown.value = 0;
    }

    public void SetShadowDistance(int index)
    {
        ShadowDistance distance = (ShadowDistance)Enum.GetValues(typeof(ShadowDistance)).GetValue(index);
        urpAsset.shadowDistance = (float)distance;
    }

    public void SetShadowResolution(int index)
    {
        ShadowResolution resolution = (ShadowResolution)Enum.GetValues(typeof(ShadowResolution)).GetValue(index);
        urpAsset.mainLightShadowmapResolution = (int)resolution;
    }

    public void SetRenderScale(int index)
    {
        RenderScale scale = (RenderScale)Enum.GetValues(typeof(RenderScale)).GetValue(index);
        urpAsset.renderScale = (int)scale / 100f;
    }

    public void ExitMenu()
    {
        gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        shadowResolutionDropdown.onValueChanged.RemoveListener(SetShadowResolution);
        shadowDistanceDropdown.onValueChanged.RemoveListener(SetShadowDistance);
        renderScaleDropdown.onValueChanged.RemoveListener(SetRenderScale);
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
}