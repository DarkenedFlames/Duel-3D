using UnityEngine;
using System;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class SphereDriver : MonoBehaviour
{
    [Header("Sphere Settings")]
    [SerializeField] float radius = 5f;

    void Awake()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        col.enabled = true;
        col.radius = radius;

        MeshRenderer mesh = GetComponent<MeshRenderer>();
        mesh.transform.localScale = 2f * radius * Vector3.one;
    }
}