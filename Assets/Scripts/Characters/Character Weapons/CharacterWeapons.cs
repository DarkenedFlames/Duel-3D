using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimationProcessor))]
[RequireComponent(typeof(CharacterStats))]
public class CharacterWeapons : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] GameObject initialWeapon;
    [SerializeField] Transform WeaponSlot;

    CharacterStats stats;
    PlayerInputDriver input;

    [NonSerialized] public GameObject currentWeapon;
    public event Action<GameObject> OnEquipWeapon;
    public event Action<Weapon> OnWeaponUsed;

    /// <summary>
    /// If <see cref="currentWeapon"/> is defined at runtime as a prefab, <see cref="Object.Instantiate()"/> and re-store it.
    /// </summary>
    void Awake()
    {
        stats = GetComponent<CharacterStats>();
        input = GetComponent<PlayerInputDriver>();

        input.OnWeaponInput += HandleWeaponInput;

        if (initialWeapon != null) EquipWeapon(initialWeapon);
    }

    /// <summary>
    /// Replaces <see cref="currentWeapon"/> with an instance of <param name="weaponPrefab">weaponPrefab</param>
    /// </summary>
    /// <param name="weaponPrefab">The weapon to be instantiated.</param>
    public void EquipWeapon(GameObject weaponPrefab)
    {        
        currentWeapon = Instantiate(weaponPrefab, WeaponSlot.position, WeaponSlot.localRotation, WeaponSlot);

        if (!currentWeapon.TryGetComponent(out IRequiresSource hasSource))
        {
            Debug.LogError("Weapon Prefab has no RequiresSource Component");
            return;
        }

        hasSource.Source = gameObject;
        OnEquipWeapon?.Invoke(currentWeapon);
    }

    void HandleWeaponInput()
    {
        Weapon weaponComponent = currentWeapon.GetComponent<Weapon>();
        WeaponDefinition weaponDefinition = weaponComponent.Definition;
        
        if (weaponComponent.TryUse())
        {
            OnWeaponUsed?.Invoke(weaponComponent);
        }
    }

    void OnDestroy()
    {
        input.OnWeaponInput -= HandleWeaponInput;
    }
}