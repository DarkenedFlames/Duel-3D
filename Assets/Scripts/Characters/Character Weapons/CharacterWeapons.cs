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
            Debug.LogError($"{name}'s {nameof(CharacterWeapons)} expected a component but it was missing: {nameof(IInputDriver)} missing!");
        else 
            input = inputDriver;

        if (initialWeapon != null)
            EquipWeapon(initialWeapon);
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {   
        if (currentWeapon != null)
        {
            Debug.Log($"{name}'s {currentWeapon.name} being replaced by {weaponPrefab.name}! Destroying {currentWeapon.name}...");
            Destroy(currentWeapon);
        }
        currentWeapon = Instantiate(weaponPrefab, WeaponSlot.position, WeaponSlot.localRotation, WeaponSlot);

        if (!currentWeapon.TryGetComponent(out Weapon weaponComponent))
        {
            Debug.LogError($"{gameObject.name}'s weapon is missing a component: {currentWeapon.name} missing {nameof(Weapon)}!");
            return;
        }
        OnEquipWeapon?.Invoke(currentWeapon);
    }

    void HandleWeaponInput()
    {
        if (currentWeapon == null) // Ignore input with no weapon equipped.
            return;

        Weapon weaponComponent = currentWeapon.GetComponent<Weapon>();
        if (weaponComponent.TryUse())
            OnWeaponUsed?.Invoke(weaponComponent);
    }

    void OnEnable() => input.OnWeaponInput += HandleWeaponInput;
    void OnDisable() => input.OnWeaponInput -= HandleWeaponInput;
}