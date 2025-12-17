using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ObjectSpawnData))]
public class ObjectSpawnDataDrawer : PropertyDrawer
{
    const float VSpace = 2f;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = 0f;

        void GetProperty(string name, out SerializedProperty prop) =>
            prop = property.FindPropertyRelative(name);

        void AddHeight(SerializedProperty prop) =>
            height += EditorGUI.GetPropertyHeight(prop, true) + VSpace;

        GetProperty("mode", out SerializedProperty modeProp);
        GetProperty("prefab", out SerializedProperty prefabProp);
        GetProperty("set", out SerializedProperty setProp);
        GetProperty("interval", out SerializedProperty intervalProp);
        GetProperty("spawnPositions", out SerializedProperty spawnPositionsProp);

        AddHeight(modeProp);

        switch ((ObjectSpawnData.Mode)modeProp.enumValueIndex)
        {
            case ObjectSpawnData.Mode.SpecificPrefab: AddHeight(prefabProp); break;
            case ObjectSpawnData.Mode.RandomPrefabFromSet: AddHeight(setProp); break;
        }

        AddHeight(intervalProp);
        AddHeight(spawnPositionsProp);
    
        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float y = position.y;

        void DrawField(SerializedProperty prop)
        {
            float h = EditorGUI.GetPropertyHeight(prop);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), prop);
            y += h + VSpace;
        }

        void GetProperty(string name, out SerializedProperty prop) =>
            prop = property.FindPropertyRelative(name);

        GetProperty("mode", out SerializedProperty modeProp);
        GetProperty("prefab", out SerializedProperty prefabProp);
        GetProperty("set", out SerializedProperty setProp);
        GetProperty("interval", out SerializedProperty intervalProp);
        GetProperty("spawnPositions", out SerializedProperty spawnPositionsProp);

        DrawField(modeProp);

        switch ((ObjectSpawnData.Mode)modeProp.enumValueIndex)
        {
            case ObjectSpawnData.Mode.SpecificPrefab: DrawField(prefabProp); break;
            case ObjectSpawnData.Mode.RandomPrefabFromSet: DrawField(setProp); break;
        }

        DrawField(intervalProp);
        DrawField(spawnPositionsProp);
    }
}