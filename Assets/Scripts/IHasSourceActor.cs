using UnityEngine;
using System;

public interface IHasSourceActor
{
    GameObject SourceActor { get; set; }
    void SetSource(GameObject actor);
}