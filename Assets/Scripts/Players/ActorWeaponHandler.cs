using System;
using UnityEngine;

public class ActorWeaponHandler : MonoBehaviour
{
    [Header("Actor Components")]
    [SerializeField] StatsHandler stats;
    [SerializeField] AnimationHandler anim;
    IInputProvider input; // Interface cannot be serialized

    [Header("Weapon Settings")]
    [SerializeField] GameObject currentWeapon;
    [SerializeField] Transform WeaponSlot;

    Weapon currentWeaponComponent;

    public event Action<Weapon> OnSwing;
    public event Action<Weapon> OnEquipWeapon;

    /// <summary>
    /// If <see cref="currentWeapon"/> is defined at runtime as a prefab, <see cref="Object.Instantiate()"/> and re-store it.
    /// </summary>
    void Awake()
    {
        input = GetComponent<IInputProvider>();
        if (currentWeapon != null) EquipWeapon(currentWeapon);
    }

    /// <summary>
    /// Replaces <see cref="currentWeapon"/> with an instance of <param name="weaponPrefab">weaponPrefab</param>
    /// </summary>
    /// <param name="weaponPrefab">The weapon to be instantiated.</param>
    public void EquipWeapon(GameObject weaponPrefab)
    {        
        currentWeapon = Instantiate(weaponPrefab, WeaponSlot.position, WeaponSlot.localRotation, WeaponSlot);

        if (!currentWeapon.TryGetComponent(out Weapon weapon))
            Debug.LogError("Weapon Prefab has no Weapon Component");

        currentWeaponComponent = weapon;
        currentWeaponComponent.SetSource(gameObject);
        OnEquipWeapon?.Invoke(currentWeaponComponent);
    }

    /// <summary>
    /// Checks for <see cref="input.AttackPressed"/> and <see cref="currentWeaponComponent.staminaCost"/>.<br/>
    /// If the confiditions are met, calls <see cref="currentWeaponComponent.TrySwing()"/>
    /// </summary>
    void Update()
    {
        if (input.AttackPressed
            && currentWeaponComponent.staminaCost <= stats.GetStat(StatType.Stamina, false)
            && currentWeaponComponent.TrySwing())
        {
            stats.TryModifyStat(StatType.Stamina, false, -1f * currentWeaponComponent.staminaCost);
            OnSwing?.Invoke(currentWeaponComponent);
        }
    }
}