using UnityEngine;
using UnityEditor;
using System.Collections.Generic;



// This could be useful, but we need to preset all tag + layer + component combinations that should be required.
// Also make it run automatically on awake/start.


public class TagComponentValidator : EditorWindow
{
    private string targetTag = "YourTagName"; // Replace with your target tag
    private readonly List<System.Type> requiredComponents = new()
    {
        typeof(Rigidbody),
        typeof(Collider),
        typeof(StatsHandler)
    }; // Add your required component types here

    [MenuItem("Tools/Validate Tag Components")]
    public static void ShowWindow()
    {
        GetWindow<TagComponentValidator>("Tag Component Validator");
    }

    void OnGUI()
    {
        GUILayout.Label("Validation Settings", EditorStyles.boldLabel);
        targetTag = EditorGUILayout.TextField("Target Tag", targetTag);

        if (GUILayout.Button("Validate Scene Objects"))
        {
            ValidateObjects();
        }
    }

    void ValidateObjects()
    {
        // Find all objects with the target tag
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(targetTag);
        bool allValid = true;

        foreach (GameObject obj in objectsWithTag)
        {
            foreach (System.Type componentType in requiredComponents)
            {
                if (obj.GetComponent(componentType) == null)
                {
                    Debug.LogError($"GameObject '{obj.name}' with tag '{targetTag}' is missing required component: '{componentType.Name}'", obj);
                    allValid = false;
                }
            }
        }

        if (allValid)
        {
            Debug.Log($"All GameObjects with tag '{targetTag}' have all required components.");
        }
        else
        {
            Debug.LogWarning("Some objects are missing required components. See console for details.");
        }
    }
}
