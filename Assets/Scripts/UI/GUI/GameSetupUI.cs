using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSetupUI : MonoBehaviour
{   
    [Header("References")]
    [SerializeField] GameSessionSettings settings;
    [SerializeField] MainMenuUI mainMenuUI;
    [SerializeField] TMP_Dropdown mapDropdown;
    [SerializeField] TextMeshProUGUI durationText;
    [SerializeField] Slider durationSlider;
    [SerializeField] Image mapIcon;

    [Header("Settings")]
    [SerializeField] int durationSliderMaxValue = 600;
    
    [Header("Map Entries")]
    [SerializeField] List<MapData> maps = new();
    
    void Start()
    {
        mapDropdown.ClearOptions();
        mapDropdown.AddOptions(maps.Select(m => m.MapName).ToList());
        mapDropdown.onValueChanged.AddListener(UpdateMapIcon);
        mapDropdown.value = 0;
        
        durationSlider.onValueChanged.AddListener(UpdateGameDuration);
        durationSlider.value = 0;
        
        durationSlider.onValueChanged.AddListener(UpdateSliderText);
        durationText.text = durationSlider.value.ToString();

        if (maps.Count != mapDropdown.options.Count)
            Debug.LogError($"{nameof(GameSetupUI)} was configured with a mismatched number of {nameof(MapData)} entries and dropdown options!");
    }

    MapData GetMapByDropdown(int index) => maps.Find(m => m.MapName == mapDropdown.options[index].text);
    
    void UpdateMapIcon(int change) => mapIcon.sprite = GetMapByDropdown(change).Icon;
    void UpdateGameDuration(float value) => settings.Duration = value * durationSliderMaxValue;
    void UpdateSliderText(float value) => durationText.text = ((int)(value * durationSliderMaxValue)).ToString();
    
    // Button hooks
    
    // +1 to account for main menu at index 0
    public void StartGame() 
    {
        SceneManager.LoadScene(GetMapByDropdown(mapDropdown.value).SceneIndex);
        Time.timeScale = 1f;
    }
    public void ReturnToMain()
    {
        Instantiate(mainMenuUI, transform.root);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        durationSlider.onValueChanged.RemoveListener(UpdateGameDuration);
        durationSlider.onValueChanged.RemoveListener(UpdateSliderText);
        mapDropdown.onValueChanged.RemoveListener(UpdateMapIcon);
    }
}