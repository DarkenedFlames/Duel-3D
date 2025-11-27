using UnityEngine;

public class SpawnContext : MonoBehaviour
{
    public Character Owner { get; set; }
    public object Spawner { get; set; }

    public void Initialize(object spawner)
    {
        Spawner = spawner; // Effect, Ability, Region, Weapon doesn't matter.
        
        Owner = spawner switch
        {
            Region => ((Region)spawner).GetComponent<SpawnContext>().Owner,                     // Region => Owner of spawning Region (may be null)
            Ability => ((Ability)spawner).GameObject.GetComponent<Character>(),                 // Ability => Owner of ability
            CharacterEffect => ((CharacterEffect)spawner).GameObject.GetComponent<Character>(), // Effect => Owner of effect
            Weapon => ((Weapon)spawner).GetComponent<SpawnContext>().Owner,                     // Weapon => Owner of the spawning Weapon (not null)
            _ => null
        };

        if (Owner == null)
        {
            Debug.LogWarning($"{name}'s {nameof(SpawnContext)} could not be initialized properly because {spawner.GetType()} is not a valid type of Spawner (or {name} is a Region whose Spawner is also a Region with a null Owner.)");
        }
    }
}
