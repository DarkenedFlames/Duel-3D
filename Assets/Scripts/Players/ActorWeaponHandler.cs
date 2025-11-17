using System;
using UnityEngine;

public class ActorWeaponHandler : MonoBehaviour
{
    public GameObject currentWeapon;
    public Transform WeaponSlot;

    private Weapon currentWeaponComponent;
    private StatsHandler stats;
    private AnimationHandler anim;
    private IInputProvider input;

    void Awake()
    {
        stats = GetComponent<StatsHandler>();
        anim = GetComponent<AnimationHandler>();
        input = GetComponent<IInputProvider>();

        if (currentWeapon != null)
            EquipWeapon(currentWeapon);
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {        
        currentWeapon = Instantiate(weaponPrefab, WeaponSlot.position, WeaponSlot.localRotation, WeaponSlot);

        if (!currentWeapon.TryGetComponent(out Weapon weapon))
            Debug.LogError("Weapon Prefab has no Weapon Component");

        currentWeaponComponent = weapon;
        weapon.SetSource(gameObject);
    }

    void Update()
    {
        if (input.AttackPressed
            && currentWeaponComponent.staminaCost <= stats.GetStat(StatType.Stamina, false)
            && currentWeaponComponent.TrySwing())
        {
            stats.TryModifyStat(StatType.Stamina, false, -1f * currentWeaponComponent.staminaCost);
            anim.TriggerAttack(currentWeaponComponent.animationTrigger); 
        }
    }
}