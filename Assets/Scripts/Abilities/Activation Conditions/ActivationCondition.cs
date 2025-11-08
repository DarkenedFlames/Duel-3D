using UnityEngine;

public abstract class ActivationCondition : ScriptableObject
{
    public abstract bool IsMet(AbilityHandler handler, Ability ability);
}
