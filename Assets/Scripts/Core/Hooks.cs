
public enum AreaHook
{
    OnTargetEnter,
    OnTargetExit,
    OnPulse,
    OnExpire,
}

public enum ProjectileHook
{
    OnTick,
    OnCollide,
    OnExpire,
}

public enum AbilityHook
{
    OnActivate,
    OnTick,
    OnEnd,
}

public enum PickupHook
{
    OnCollide
}

public enum EffectHook
{
    OnApply,
    OnStackGained,
    OnRefresh,
    OnPulse,
    OnStackLost,
    OnExpire,
}

public enum WeaponHook
{
    OnSwing,
    OnCollide,
}