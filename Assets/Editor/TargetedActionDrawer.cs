using System;
using UnityEditor;

[CustomPropertyDrawer(typeof(ITargetedAction), true)] 
public class TargetedActionDrawer : PolymorphicInterfaceDrawer 
{ 
    protected override Type InterfaceType => typeof(ITargetedAction);
}
