using UnityEngine;

[System.Serializable]
public class AModifyEffect : IGameAction
{
    [Header("Effect Configuration")]
    [Tooltip("Effect to modify."), SerializeField]
    EffectDefinition effectDefinition;
    
    [Tooltip("Apply stacks if mode is checked, otherwise remove stacks."), SerializeField]
    bool mode;

    [Tooltip("The number of stacks to apply or remove."), SerializeField]
    int stacks;
    
    public void Execute(GameObject source, GameObject target)
    {
        if (target == null) return;

        if (target.TryGetComponent(out CharacterEffects effects))
            if (mode) 
                effects.AddEffect(effectDefinition, stacks);
            else
                effects.RemoveEffect(effectDefinition, stacks);
    }
}