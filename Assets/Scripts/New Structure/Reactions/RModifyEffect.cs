using UnityEngine;

public class RModifyEffect : Reaction
{
    [Header("ModifyEffect Configuration")]
    [Tooltip("Effect to modify."), SerializeField]
    GameObject effectPrefab;
    
    [Tooltip("Apply stacks if mode is checked, otherwise remove stacks."), SerializeField]
    bool mode;

    [Tooltip("The number of stacks to apply or remove."), SerializeField]
    int stacks;
    
    public void ModifyEffect(GameObject target)
    {
        if (target == null) return;

        if (target.TryGetComponent(out EffectHandler effects))
            if (mode) 
                effects.ApplyEffect(effectPrefab, stacks);
            else
                effects.RemoveStacks(effectPrefab.name, stacks);
    }
}