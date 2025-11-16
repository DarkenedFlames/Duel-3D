using UnityEngine;

public interface IInstructionContext
{
    GameObject Actor { get; }
    MonoBehaviour Domain { get; }
}


public class AreaContext : IInstructionContext
{
    public GameObject Actor { get; }
    public MonoBehaviour Domain { get; }

    public AreaContext(Area area, GameObject target)
    {
        Domain = area;
        Actor = target;
    }
}

public class ProjectileContext : IInstructionContext
{
    public GameObject Actor { get; }
    public MonoBehaviour Domain { get; }
    public ProjectileContext(Projectile projectile, GameObject target)
    {
        Domain = projectile;
        Actor = target;
    }

}

public class AbilityContext : IInstructionContext
{
    public GameObject Actor { get; private set; }
    public MonoBehaviour Domain { get; private set; }

    public AbilityContext(AbilityHandler abilityHandler, GameObject caster)
    {
        Domain = abilityHandler;
        Actor = caster;
    }
}


public class PickupContext : IInstructionContext
{
    public GameObject Actor { get; private set; }
    public MonoBehaviour Domain { get; private set; }

    public PickupContext(Pickup pickup, GameObject target)
    {
        Domain = pickup;
        Actor = target;
    }
}

public class EffectContext : IInstructionContext
{
    public GameObject Actor { get; private set; }
    public MonoBehaviour Domain { get; private set; }

    public EffectContext(EffectHandler effectHandler, GameObject target)
    {
        Domain = effectHandler;
        Actor = target;
    }
}

public class WeaponContext : IInstructionContext
{
    public GameObject Actor { get; private set; }
    public MonoBehaviour Domain { get; private set; }

    public WeaponContext(Weapon weapon, GameObject target)
    {
        Domain = weapon;
        Actor = target;
    }
}
