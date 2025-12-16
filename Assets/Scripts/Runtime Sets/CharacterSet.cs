using UnityEngine;
using HBM.Scriptable;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Character Set", menuName = "Runtime Sets/Character")]
public class CharacterSet : RuntimeSet<Character>
{
    public bool TryGetSinglePlayer(out Character singlePlayer)
    {
        singlePlayer = null;
        foreach (Character character in this)
            if (character.CompareTag("Player"))
                singlePlayer = character;
            
        return singlePlayer != null;
    }

    public Character GetClosest(Vector3 position, out float distance)
    {
        distance = float.PositiveInfinity;
        Character closest = null;

        foreach (Character character in this)
        {
            if (character == null) continue;

            float newDistance = Vector3.Distance(character.transform.position, position);
            if (newDistance < distance)
            {
                distance = newDistance;
                closest = character;
            }
        }

        return closest;
    }

    public Character GetClosestExcluding(Vector3 position, Character exclude, out float distance)
    {
        Character closest = null;
        distance = float.MaxValue;

        foreach (Character c in this)
        {
            if (c == exclude) continue;

            float d = Vector3.Distance(position, c.transform.position);
            if (d < distance)
            {
                distance = d;
                closest = c;
            }
        }

        return closest;
    }

    public Character GetClosestExcludingMany(Vector3 position, List<Character> exclusions, out float distance)
    {
        Character closest = null;
        distance = float.MaxValue;

        foreach (Character c in this)
        {
            if (exclusions.Contains(c)) continue;

            float d = Vector3.Distance(position, c.transform.position);
            if (d < distance)
            {
                distance = d;
                closest = c;
            }
        }

        return closest;
    }
}
