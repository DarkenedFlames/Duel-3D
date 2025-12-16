using UnityEngine;

public class Ability : IActionSource
{
    public Character Owner { get; set; }
    public Transform Transform => GameObject.transform;
    public GameObject GameObject { get; set; }

    public AbilityDefinition Definition { get; private set; }
    readonly FloatCounter seconds;

    public Ability(Character owner, AbilityDefinition definition)
    {
        Owner = owner;
        GameObject = owner.gameObject;

        Definition = definition;
        seconds = new(0, 0, Definition.cooldown, true, true);
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
        
        seconds.Reset();
        return true;
    }
}
