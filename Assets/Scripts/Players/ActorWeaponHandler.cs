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

    WeaponSwing swingComponent;

    public event Action<WeaponSwing> OnSwing;
    public event Action<GameObject> OnEquipWeapon;

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

        if (!currentWeapon.TryGetComponent(out IHasSourceActor hasSource))
        {
            Debug.LogError("Weapon Prefab has no IHasSourceActor Component");
            return;
        }

        if (!currentWeapon.TryGetComponent(out WeaponSwing swing))
        {
            Debug.LogError("Weapon Prefab has no WeaponSwing Component");
            return;
        }

        hasSource.SetSource(gameObject);
        swingComponent = swing;

        OnEquipWeapon?.Invoke(currentWeapon);
    }

    void Update()
    {
        if (input.AttackPressed
            && swingComponent.swingStaminaCost <= stats.GetStat(StatType.Stamina, false)
            && swingComponent.TrySwing())
        {
            stats.TryModifyStat(StatType.Stamina, false, -1f * swingComponent.swingStaminaCost);
            OnSwing?.Invoke(swingComponent);
        }
    }
}