using UnityEngine;

public class TodoGizmo : MonoBehaviour
{
    public string todoMessage = "TODO: Implement this feature";

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawIcon(transform.position, "d_UnityEditor.ConsoleWindow", true);
        #if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.5f, todoMessage);
        #endif
    }
}