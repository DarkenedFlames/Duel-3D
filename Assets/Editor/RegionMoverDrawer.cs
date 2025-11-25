using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(IRegionMover), true)]
public class RegionMoverDrawer : PolymorphicInterfaceDrawer
{
    protected override Type InterfaceType => typeof(IRegionMover);
}
