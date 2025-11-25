using HBM.Scriptable;
using UnityEngine;

[RequireComponent(typeof(Region))]
public class RegionSetRegistrar : SetRegistarBase<Region>
{
    protected override Region _object => GetComponent<Region>();
}

