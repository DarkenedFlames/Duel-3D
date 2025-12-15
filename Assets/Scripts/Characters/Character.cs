using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(CharacterAbilities))]
[RequireComponent(typeof(CharacterWeapons))]
[RequireComponent(typeof(CharacterEffects))]
[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterResources))]
[RequireComponent(typeof(CharacterAnimation))]
[RequireComponent(typeof(IInputDriver))]

public class Character : MonoBehaviour
{
    public CharacterStats CharacterStats;
    public CharacterAbilities CharacterAbilities;
    public CharacterWeapons CharacterWeapons;
    public CharacterEffects CharacterEffects;
    public CharacterMovement CharacterMovement;
    public CharacterResources CharacterResources;
    public CharacterAnimation CharacterAnimation;
    public IInputDriver CharacterInput => GetComponent<IInputDriver>();
}