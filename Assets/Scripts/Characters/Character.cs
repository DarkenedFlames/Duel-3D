using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(CharacterAbilities))]
[RequireComponent(typeof(CharacterWeapons))]
[RequireComponent(typeof(CharacterEffects))]
[RequireComponent(typeof(CharacterStatuses))]
[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterResources))]
public class Character : MonoBehaviour
{
    public CharacterStats CharacterStats => GetComponent<CharacterStats>();
    public CharacterAbilities CharacterAbilities => GetComponent<CharacterAbilities>();
    public CharacterWeapons CharacterWeapons => GetComponent<CharacterWeapons>();
    public CharacterEffects CharacterEffects => GetComponent<CharacterEffects>();
    public CharacterStatuses CharacterStatuses => GetComponent<CharacterStatuses>();
    public CharacterMovement CharacterMovement => GetComponent<CharacterMovement>();
    public CharacterResources CharacterResources => GetComponent<CharacterResources>();
}