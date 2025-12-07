using UnityEngine;
using System;

[Serializable]
public class FloatVector3Pair
{
    public float Float;
    public Vector3 Vector;

    public FloatVector3Pair(float _float, Vector3 vector)
    {
        Float = _float;
        Vector = vector;
    }
}