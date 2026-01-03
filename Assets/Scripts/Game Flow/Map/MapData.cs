using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "Game Flow/Map Data", order = 2)]
public class MapData : ScriptableObject
{
    [Tooltip("The name of the map.")]
    public string MapName = string.Empty;

    [Tooltip("The scene index in the build settings that corresponds to this map."), Min(1)]
    public int SceneIndex = 1;
    
    [Tooltip("The icon representing this map in the UI.")]
    public Sprite Icon;
}