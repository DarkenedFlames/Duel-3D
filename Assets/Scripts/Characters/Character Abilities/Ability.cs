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

        if(!Owner.CharacterResources.TryGetResource(Definition.expendedResource, out CharacterResource resource))
        {
            Debug.LogError($"{GameObject.name}'s Ability {Definition.abilityName} expected {GameObject.name} to have {Definition.expendedResource.ResourceName} but it was missing!");
            return false;
        }

        Owner.CharacterResources.ChangeResourceValue(
            Definition.expendedResource,
            -1f * Definition.resourceCost,
            out float _,
            true
        );
        
        seconds.Reset();
        ActionContext context = new(){ Source = this, Target = Owner };
        Definition.actions.ForEach(a => a.Execute(context));
        return true;
    }
}
