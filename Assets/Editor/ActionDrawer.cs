using System;
using UnityEditor;

[CustomPropertyDrawer(typeof(IGameAction), true)] 
public class ActionDrawer : PolymorphicInterfaceDrawer 
{ 
    protected override Type InterfaceType => typeof(IGameAction);
}