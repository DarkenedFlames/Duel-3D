using UnityEngine;

public class Ability : IActionSource
{
    public Character Owner { get; set; }
    public Transform Transform => GameObject.transform;
    public GameObject GameObject { get; set; }

    public AbilityDefinition Definition { get; private set; }
    public readonly FloatCounter seconds;

    public int Rank = 1;

    public Ability(Character owner, AbilityDefinition definition)
    {
        Owner = owner;
        GameObject = owner.gameObject;

        Definition = definition;
        seconds = new(0, 0, Definition.cooldown, true, true);
    }

    public bool TryRankUp()
    {
        if (Rank >= Definition.maxRank) return false;
        Rank++;
        return true;
    }
    
    public void TickCooldown(float dt) => seconds.Decrease(dt);

    public bool TryActivate()
    {
        if (!seconds.Expired) return false;

        CharacterResource resource = Owner.CharacterResources.GetResource(Definition.ExpendedResource);
        
        if (resource.Value < Definition.resourceCost) return false;
        if (resource.Definition.resourceType == ResourceType.Health && resource.Value == Definition.resourceCost) return false;

        Definition.ExecuteActions(AbilityHook.OnCast, new(){ Source = this, Target = Owner });

        Owner.CharacterResources.ChangeResourceValue(
            Definition.ExpendedResource,
            -1f * Definition.resourceCost,
            out float _,
            true
        );
    
        seconds.SetMax(CurrentCooldown());
        seconds.Reset();
        return true;
    }

    public float CurrentCooldown() => Definition.cooldown * 100f / (100f + Owner.CharacterStats.GetStat(StatType.CooldownReduction).Value);
}
