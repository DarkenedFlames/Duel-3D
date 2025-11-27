using System;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(Character))]
public class CharacterWeapons : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] GameObject initialWeapon;
    [SerializeField] Transform WeaponSlot;

    IInputDriver input;

    [NonSerialized] public GameObject currentWeapon;
    public event Action<GameObject> OnEquipWeapon;
    public event Action<Weapon> OnWeaponUsed;

    void Awake()
    {

        if (!TryGetComponent(out IInputDriver inputDriver))
            Debug.LogError($"{name}'s {nameof(CharacterWeapons)} expected an implementer of {nameof(IInputDriver)} but none was found!");
        else input = inputDriver;

        if (initialWeapon != null) EquipWeapon(initialWeapon);
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {   
        if (currentWeapon != null)
        {
            Debug.Log($"{name}'s {currentWeapon.name} being replaced by {weaponPrefab.name}! Destroying {currentWeapon.name}...");
            Destroy(currentWeapon);
        }
        currentWeapon = Instantiate(weaponPrefab, WeaponSlot.position, WeaponSlot.localRotation, WeaponSlot);

        if (!currentWeapon.TryGetComponent<SpawnContext>(out var spawnContext))
        {
            Debug.LogError($"{name}'s weapon was instantiated missing a component: {currentWeapon.name} missing {nameof(SpawnContext)}! Destroying {currentWeapon.name}...");
            Destroy(currentWeapon);
            return;
        }

        spawnContext.Owner = GetComponent<Character>();
        spawnContext.Spawner = null;
        OnEquipWeapon?.Invoke(currentWeapon);
    }

    void HandleWeaponInput()
    {
        if (currentWeapon == null) return; // Ignore weapon input with no weapon equipped.
        
        if (!currentWeapon.TryGetComponent(out Weapon weaponComponent))
        {
            Debug.LogError($"{name}'s weapon is missing a component: {currentWeapon.name} missing {nameof(Weapon)}!");
            return;
        }

        if (weaponComponent.TryUse())
            OnWeaponUsed?.Invoke(weaponComponent);
    }

    void OnEnable() => input.OnWeaponInput += HandleWeaponInput;
    void OnDisable() => input.OnWeaponInput -= HandleWeaponInput;
}