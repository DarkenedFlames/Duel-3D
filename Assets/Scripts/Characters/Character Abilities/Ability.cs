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
        foreach (ActivationCondition condition in Definition.activationConditions)
            if (!condition.IsMet(this)) return false;

        if (!seconds.Expired) return false;

        CharacterResource resource = Owner.CharacterResources.GetResource(Definition.ExpendedResource, this);
        
        if (resource.Value < Definition.resourceCost) return false;

        Owner.CharacterResources.ChangeResourceValue(
            Definition.ExpendedResource,
            -1f * Definition.resourceCost,
            out float _,
            true
        );
        
        seconds.Reset();
        
        ActionContext context = new(){ Source = this, Target = Owner };
        Definition.ExecuteActions(AbilityHook.OnCast, context);
        
        return true;
    }
}
