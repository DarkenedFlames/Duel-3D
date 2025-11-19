using UnityEngine;

public abstract class ActivationCondition : ScriptableObject
{
    public abstract bool IsMet(Ability ability);
}
