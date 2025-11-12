using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponHandler : MonoBehaviour
{
    [Header("Weapon Definition")]
    public WeaponDefinition weaponDefinition;

    [NonSerialized] public GameObject wielder;
    private IInputProvider input;

    Weapon weapon;

    /// <summary>
    /// Sets defaults and creates and stores logical Weapon.<br/>
    /// <see cref="SpawnerController.SpawnWeapon"/> ensures <see cref="weaponDefinition"/> is not null before this.
    /// </summary>
    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Collider>().enabled = true;

        weapon = new Weapon(weaponDefinition, this);
    }

    /// <summary>
    /// Stores wielder, attaches transform to socket.<br/>
    /// <see cref="SpawnerController.SpawnWeapon"/>  ensures this is called after <see cref="Awake"/>. 
    /// </summary>
    public void Spawn(GameObject wielderObject)
    {
        // Need to make sure SpawnController validates wielder.
        wielder = wielderObject;
        input = wielder.GetComponent<IInputProvider>();

        // This whole section is a bit messy.

        // Parent to hand
        string socketName = "weapon_socket";
        Transform hand = FindDeepChild(wielder.transform, socketName);
        if (hand != null)
        {
            transform.SetParent(hand);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogError($"{wielder.name} has no '{socketName}' transform for weapon attachment!");
            Destroy(gameObject);
            return;
        }

        weapon.PerformHook(weapon.equipGate, b => b.OnSpawn(), nameof(Spawn));
    }

    public static Transform FindDeepChild(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            var result = FindDeepChild(child, name); // Sick recursion
            if (result != null) return result;
        }
        return null;
    }

    void Update()
    {
        float dt = Time.deltaTime;
        weapon.GetAllGates().ForEach(g => g.Tick(dt));
        weapon.PerformHook(weapon.updateGate, b => b.OnUpdate(dt), nameof(Update));
        if (input.AttackPressed)
            Attack();
    }
    void OnTriggerEnter(Collider other)
    {
        if (wielder != null && other.transform.IsChildOf(wielder.transform)) return;
        weapon.PerformHook(weapon.triggerGate, b => b.OnTrigger(other), nameof(OnTriggerEnter));
    }
    public void Attack() => weapon.PerformHook(weapon.attackGate, b => b.OnAttack(), nameof(Attack));

}
