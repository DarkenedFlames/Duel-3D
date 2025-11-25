using HBM.Scriptable;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterSetRegistrar : SetRegistarBase<Character>
{
    protected override Character _object => GetComponent<Character>();
}

