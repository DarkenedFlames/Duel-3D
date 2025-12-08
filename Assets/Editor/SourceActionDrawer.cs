using System;
using UnityEditor;

[CustomPropertyDrawer(typeof(ISourceAction), true)] 
public class SourceActionDrawer : PolymorphicInterfaceDrawer 
{ 
    protected override Type InterfaceType => typeof(ISourceAction);
}
