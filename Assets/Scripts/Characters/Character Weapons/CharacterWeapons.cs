using System;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
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
        input = GetComponent<IInputDriver>();

        if (initialWeapon != null) EquipWeapon(initialWeapon);
    }

    public void EquipWeapon(GameObject weaponPrefab, GameObject spawner = null)
    {        
        currentWeapon = Instantiate(weaponPrefab, WeaponSlot.position, WeaponSlot.localRotation, WeaponSlot);

        if (!currentWeapon.TryGetComponent<SpawnContext>(out var spawnContext))
            Debug.Log("Weapon has no SpawnContext");

        spawnContext.Owner = gameObject;
        spawnContext.Spawner = spawner;
        OnEquipWeapon?.Invoke(currentWeapon);
    }

    void HandleWeaponInput()
    {
        if (currentWeapon == null) return;
        Weapon weaponComponent = currentWeapon.GetComponent<Weapon>();
                
        if (weaponComponent.TryUse())
            OnWeaponUsed?.Invoke(weaponComponent);
    }

    void OnEnable() => input.OnWeaponInput += HandleWeaponInput;
    void OnDisable() => input.OnWeaponInput -= HandleWeaponInput;
    
}