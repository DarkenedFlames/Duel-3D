using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(ObjectSpawner))]
public class ObjectSpawnerEditor : Editor
{
    void OnSceneGUI()
    {
        ObjectSpawner spawner = (ObjectSpawner)target;
        
        // Use reflection to access the private data list
        FieldInfo dataField = typeof(ObjectSpawner).GetField("data", BindingFlags.NonPublic | BindingFlags.Instance);
        if (dataField == null) return;
        
        var dataList = dataField.GetValue(spawner) as System.Collections.Generic.List<ObjectSpawnData>;
        if (dataList == null) return;

        // Track if any changes were made
        bool changed = false;

        // Iterate through each ObjectSpawnData in the list
        for (int dataIndex = 0; dataIndex < dataList.Count; dataIndex++)
        {
            ObjectSpawnData data = dataList[dataIndex];
            
            // Use reflection to access the private spawnPositions list
            FieldInfo positionsField = typeof(ObjectSpawnData).GetField("spawnPositions", BindingFlags.NonPublic | BindingFlags.Instance);
            if (positionsField == null) continue;
            
            var positions = positionsField.GetValue(data) as System.Collections.Generic.List<Vector3>;
            if (positions == null) continue;

            // Draw handles for each spawn position
            for (int i = 0; i < positions.Count; i++)
            {
                EditorGUI.BeginChangeCheck();
                
                // Draw a position handle
                Vector3 newPosition = Handles.PositionHandle(positions[i], Quaternion.identity);
                
                // Add a label showing the index
                Handles.Label(positions[i] + Vector3.up * 0.7f, $"Spawn {dataIndex}.{i}");
                
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(spawner, "Move Spawn Point");
                    positions[i] = newPosition;
                    changed = true;
                }
            }
        }

        if (changed)
        {
            EditorUtility.SetDirty(spawner);
        }
    }
}
