using System;
using UnityEngine;

[Serializable]
public class GivesWeaponInstruction : IInstruction
{
    [Tooltip("Weapon to give.")]
    public GameObject weaponPrefab;
    public void Execute(IInstructionContext context)
    {
        if (context.Actor.TryGetComponent(out ActorWeaponHandler weaponHandler))
            weaponHandler.EquipWeapon(weaponPrefab);
    }
}