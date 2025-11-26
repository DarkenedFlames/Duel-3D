using UnityEngine;

public class SpawnContext : MonoBehaviour
{
    public Character Owner { get; set; }
    public object Spawner { get; set; }

    public void Initialize(object spawner)
    {
        Spawner = spawner; // Effect, Ability, Region, doesn't matter.
        
        Owner = spawner switch
        {
            Region => ((Region)spawner).GetComponent<SpawnContext>().Owner,                     // Region => Owner of spawning Region (may be null)
            Ability => ((Ability)spawner).GameObject.GetComponent<Character>(),                 // Ability => Owner of ability
            CharacterEffect => ((CharacterEffect)spawner).GameObject.GetComponent<Character>(), // Effect => Owner of effect
            _ => null
        };

        if (Owner == null)
        {
            Debug.LogError($"{name}'s SpawnContext could not be initialized properly because {spawner.GetType()} is not a valid type of Spawner (or {name} is a Region whose Spawner is also a Region with a null Owner.)");
        }
    }
}
